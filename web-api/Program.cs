using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using System.Text;
using Vueboard.Api;
using Vueboard.Api.GraphQL;
using Vueboard.DataAccess.Repositories;
using Vueboard.DataAccess.Repositories.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Minimal configuration - in real project use configuration providers and secrets
builder.Services.AddLogging();

// JWT Bearer authentication (validate token signature/claims) - configuration via env vars
var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? "https://supabase.local";
var jwtAudience = builder.Configuration["JWT:Audience"] ?? "vueboard";
var jwtSecret = builder.Configuration["JWT:Secret"] ?? "ReplaceWithSupabaseJWTSecretForLocalDevOnly";

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidIssuer = jwtIssuer,
    ValidateAudience = true,
    ValidAudience = jwtAudience,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
    ValidateLifetime = true,
    ClockSkew = TimeSpan.FromSeconds(30)
  };
});

// Register application services and simple in-memory repositories
builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
builder.Services.AddSingleton<IProjectColumnRepository, InMemoryProjectColumnRepository>();
builder.Services.AddSingleton<IWorkItemRepository, InMemoryWorkItemRepository>();

// Register DataLoaders
builder.Services.AddDataLoader<ProjectByIdDataLoader>();
builder.Services.AddDataLoader<ProjectColumnsByProjectIdDataLoader>();

// Configure GraphQL
builder.Services.AddAuthorization();
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<ProjectType>()
    .AddType<ProjectColumnType>()
    .AddType<WorkItemType>()
    .AddDataLoader<ProjectByIdDataLoader>()
    .AddDataLoader<ProjectColumnsByProjectIdDataLoader>()
    .ModifyRequestOptions(opts => opts.IncludeExceptionDetails = true);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
// Playground for local dev
app.MapGet("/", () => Results.Redirect("/graphql"));

app.Run();

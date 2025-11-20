using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vueboard.Api.Auth;
using Vueboard.Api.GraphQL;
using Vueboard.DataAccess.EntityFramework;
using Vueboard.DataAccess.EntityFramework.Config;
using Vueboard.DataAccess.EntityFramework.Repositories;
using Vueboard.DataAccess.Repositories;
using Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots;
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
builder.Services.AddAuthorization();
builder.Services.AddSingleton<JwtSessionInterceptor>();

// Register EF Context and config provider
builder.Services.AddSingleton<IVueboardDbContextConfigProvider, VueboardDbContextEnvConfigProvider>();
builder.Services.AddDbContext<IVueboardDbContext, VueboardDbContext>((serviceProvider, options) =>
{
  // Establish DB connection params using injected config provider
  var configProvider = serviceProvider.GetRequiredService<IVueboardDbContextConfigProvider>();
  var dbConfig = configProvider.Provide();
  dbConfig.Apply(options);

  // Configure interceptor to forward JWT claims to Postgres
  var interceptor = serviceProvider.GetRequiredService<JwtSessionInterceptor>();
  options.AddInterceptors(interceptor);
});

// TODO: Implement EF repositories and replace in-memory ones here.
// Register Repositories
builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
builder.Services.AddSingleton<IProjectColumnRepository, InMemoryProjectColumnRepository>();
builder.Services.AddSingleton<IWorkItemRepository, InMemoryWorkItemRepository>();
builder.Services.AddSingleton<IWorkItemTagRepository, InMemoryWorkItemTagRepository>();

// builder.Services.AddScoped<IProjectRepository, EFProjectRepository>();
// builder.Services.AddScoped<IProjectColumnRepository, EFProjectColumnRepository>();
// builder.Services.AddScoped<IWorkItemRepository, EFWorkItemRepository>();
// builder.Services.AddScoped<IWorkItemTagRepository, EFWorkItemTagRepository>();

// Register QueryRoots
builder.Services.AddScoped<IProjectQueryRoot, ProjectQueryRoot>();
builder.Services.AddScoped<IProjectColumnQueryRoot, ProjectColumnQueryRoot>();
builder.Services.AddScoped<IWorkItemQueryRoot, WorkItemQueryRoot>();
builder.Services.AddScoped<IWorkItemTagQueryRoot, WorkItemTagQueryRoot>();

// Register DataLoaders
builder.Services.AddDataLoader<ProjectByIdDataLoader>();
builder.Services.AddDataLoader<ProjectColumnsByProjectIdDataLoader>();
builder.Services.AddDataLoader<WorkItemsByProjectColumnIdDataLoader>();

// Configure GraphQL
builder.Services.AddGraphQLServer()
  .AddQueryType<Query>()
  .AddMutationType<Mutation>()
  .AddType<ProjectType>()
  .AddType<ProjectColumnType>()
  .AddType<WorkItemType>()
  .AddDataLoader<ProjectByIdDataLoader>()
  .AddDataLoader<ProjectColumnsByProjectIdDataLoader>()
  .AddDataLoader<WorkItemsByProjectColumnIdDataLoader>()
  .ModifyRequestOptions(opts => opts.IncludeExceptionDetails = true);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
// Playground for local dev
app.MapGet("/", () => Results.Redirect("/graphql"));

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Vueboard.Api.Auth;
using Vueboard.Api.GraphQL;
using Vueboard.DataAccess.Repositories.EntityFramework;
using Vueboard.DataAccess.Repositories.EntityFramework.Config;
using Vueboard.DataAccess.Repositories;
using Vueboard.DataAccess.Repositories.EntityFramework.QueryRoots;
// using Vueboard.DataAccess.Repositories.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Minimal configuration - in real project use configuration providers and secrets
builder.Services.AddLogging();

// JWT Bearer authentication (validate token signature/claims) - configuration via env vars
var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? "http://localhost:54321";
var jwtAudience = builder.Configuration["JWT:Audience"] ?? "authenticated";
var jwtSecret = builder.Configuration["JWT:Secret"] ?? "super-secret-jwt-token-with-at-least-32-characters-long";

// HINT: To get the JWT Secret from Supabase, you can run `show app.settings.jwt_secret;` in the SQL Editor tab.

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
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<JwtSessionInterceptor>();
builder.Services.AddScoped<IUserIdAccessor, SupabaseUserIdAccessor>();

// CORS configuration
var allowedOrigins = builder.Configuration["CORS_ALLOWED_ORIGINS"] ?? "";
string[] originsArray = allowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
builder.Services.AddCors(options =>
{
  options.AddPolicy("VueboardCorsPolicy", policy =>
  {
    if (originsArray.Length > 0)
    {
      policy.WithOrigins(originsArray)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    }
    else
    {
      policy.WithOrigins(["http://localhost:3000"])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    }
  });
});

// Register EF Context and config provider
builder.Services.AddScoped<IVueboardDbContextConfigProvider, VueboardDbContextEnvPostgresConfigProvider>();
builder.Services.AddDbContext<IVueboardDbContext, VueboardDbContext>((serviceProvider, options) =>
{
  // Establish DB connection params using injected config provider
  var configProvider = serviceProvider.GetRequiredService<IVueboardDbContextConfigProvider>();
  var dbConfig = configProvider.Provide();
  dbConfig.Apply(options);

  // Configure interceptor to forward JWT claims to Postgres
  var interceptor = serviceProvider.GetRequiredService<JwtSessionInterceptor>();
  options.AddInterceptors(interceptor);
}, ServiceLifetime.Scoped);

// Register Repositories
// builder.Services.AddSingleton<IProjectRepository, InMemoryProjectRepository>();
// builder.Services.AddSingleton<IProjectColumnRepository, InMemoryProjectColumnRepository>();
// builder.Services.AddSingleton<IWorkItemRepository, InMemoryWorkItemRepository>();
// builder.Services.AddSingleton<IWorkItemTagRepository, InMemoryWorkItemTagRepository>();

builder.Services.AddScoped<IProjectRepository, EFProjectRepository>();
builder.Services.AddScoped<IProjectColumnRepository, EFProjectColumnRepository>();
builder.Services.AddScoped<IWorkItemRepository, EFWorkItemRepository>();
builder.Services.AddScoped<IWorkItemTagRepository, EFWorkItemTagRepository>();

// Register QueryRoots
builder.Services.AddScoped<IProjectQueryRoot, ProjectQueryRoot>();
builder.Services.AddScoped<IProjectColumnQueryRoot, ProjectColumnQueryRoot>();
builder.Services.AddScoped<IWorkItemQueryRoot, WorkItemQueryRoot>();
builder.Services.AddScoped<IWorkItemTagQueryRoot, WorkItemTagQueryRoot>();

// TODO: Remove these lines if not needed, due to data loader reg below
// Register DataLoaders
// builder.Services.AddDataLoader<ProjectByIdDataLoader>();
// builder.Services.AddDataLoader<ProjectColumnsByProjectIdDataLoader>();
// builder.Services.AddDataLoader<WorkItemsByProjectColumnIdDataLoader>();
// builder.Services.AddDataLoader<WorkItemTagsByWorkItemIdDataLoader>();

// Configure GraphQL
builder.Services.AddGraphQLServer()
  .AddQueryType<Query>()
  .AddMutationType<Mutation>()
  .AddType(new UuidType('D'))
  .AddType<ProjectType>()
  .AddType<ProjectColumnType>()
  .AddType<WorkItemType>()
  .AddType<WorkItemTagType>()
  .AddDataLoader<ProjectByIdDataLoader>()
  .AddDataLoader<ProjectColumnsByProjectIdDataLoader>()
  .AddDataLoader<WorkItemsByProjectColumnIdDataLoader>()
  .AddDataLoader<WorkItemTagsByWorkItemIdDataLoader>()
  .ModifyRequestOptions(opts =>
  {
    // TODO: Only enable this when in dev mode (use env vars to determine this)
    opts.IncludeExceptionDetails = true;
  });

var app = builder.Build();

app.UseRouting();
app.UseCors("VueboardCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();
// Playground for local dev
app.MapGet("/", () => Results.Redirect("/graphql"));

app.Run();

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackOverflowAPI_BAL;
using StackOverflowAPI_BAL.Extentions;
using StackOverflowAPI_BAL.Filters;
using StackOverflowAPI_DAL.Contracts;
using StackOverflowAPI_DAL.Data;
using StackOverflowAPI_DAL.LoggerServices;
using StackOverflowAPI_DAL.Models;
using StackOverflowAPI_DAL.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
builder.Services.AddScoped<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IRepositeryManager, RepositoryManager>();
builder.Services.AddScoped<IQuestionRepositery, QuestionRepositery>();
builder.Services.AddScoped<ICommentRepositery, CommentRepositery>();
builder.Services.AddScoped<ITagRepositery, TagRepositery>();
builder.Services.AddScoped<RepositeryBase<Question>, QuestionRepositery>();
builder.Services.AddScoped<RepositeryBase<Comment>, CommentRepositery>();
builder.Services.AddScoped<RepositeryBase<Tag>, TagRepositery>();
builder.Services.AddScoped<ValidationFilterAttribute>();
var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(
    opts =>
    {
        opts.UseSqlServer(connectionString,
    b => b.MigrationsAssembly("StackOverflowAPI"));
    });
var authBuilder = builder.Services.AddIdentityCore<User>(o =>
{
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 8;
    o.User.RequireUniqueEmail = true;
});
authBuilder = new IdentityBuilder(authBuilder.UserType, typeof(IdentityRole), authBuilder.Services);
authBuilder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// Add services to the container.
var jwtSettings =builder.Configuration.GetSection("JwtSettings");
var secretKey = Environment.GetEnvironmentVariable("SECRET");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
            ValidAudience = jwtSettings.GetSection("validAudience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
        };
    })
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"), "jwtBearerScheme2");
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddResponseCaching();
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.Configure<IISOptions>(options => { });
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers(
    config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
})
.AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
).AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Place to add JWT with Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer" 
    });
s.AddSecurityRequirement(new OpenApiSecurityRequirement()
{

    { 
        new OpenApiSecurityScheme 
        {
            Reference = new OpenApiReference
            { 
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Name = "Bearer",
        },
        new List<string>()
    }
});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}
LoggerManager logger=new LoggerManager();
app.ConfigureExceptionHandler(logger);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.MapControllers();

app.Run();

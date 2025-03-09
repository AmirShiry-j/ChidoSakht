using Application.ConfigService;
using Infrastructure.ConfigService;
using Application.Messagers.EmailService;
using Infrastructure.Messagers.EmailService;
using ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Contexts;
using Application.Messagers.SmsService;
using Infrastructure.Messagers.SmsService;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using WebApi.Tools.PersianError;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Helpers;
using WebApi.Tools.TokenValidator;
using Application.TokenService;
using Application.Interfaces.Localization;
using Infrastructure.Localization;
using WebApi.Filters.Language;

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = builder.Configuration;

//Nlog configs
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();


// Add Localization and set their configs
builder.Services.AddLocalization(options => options.ResourcesPath = "Infrastructure/Localization");


//Add CORS configs
//Get origins cores in appsetting
var corsOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b.WithOrigins(corsOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Add services to the container.

//Config controller service
builder.Services.AddControllers();

//Config Vesioning
builder.Services.AddApiVersioning(option =>
{
    option.ReportApiVersions = true;
});

//Config DataBase
#region Connect To DataBase
string connection = Configuration["DatabaseSettings:ConnectionString"];
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(connection));
#endregion


//Config Identity and his option
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders()
    .AddRoles<Role>()
    .AddErrorDescriber<PersianIdentityErrors>();

//Set Identity's Options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = false;

    options.Password.RequireDigit = Configuration.GetSection("IdentitySettings:RequireDigit").Get<bool>();
    options.Password.RequiredLength = Configuration.GetSection("IdentitySettings:RequiredLength").Get<int>(); 
    options.Password.RequireLowercase = Configuration.GetSection("IdentitySettings:RequireLowercase").Get<bool>();
    options.Password.RequireUppercase = Configuration.GetSection("IdentitySettings:RequireUppercase").Get<bool>(); 
    options.Password.RequireNonAlphanumeric = Configuration.GetSection("IdentitySettings:RequireNonAlphanumeric").Get<bool>();
    options.Password.RequiredUniqueChars = Configuration.GetSection("IdentitySettings:RequiredUniqueChars").Get<int>();
    options.Lockout.MaxFailedAccessAttempts = Configuration.GetSection("IdentitySettings:MaxFailedAccessAttempts").Get<int>();
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Configuration.GetSection("IdentitySettings:DefaultLockoutTimeSpan").Get<int>());

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.SignIn.RequireConfirmedAccount = false;


});

// Localization service
builder.Services.AddScoped<ILocalizationService, LocalizationService>();

//Config service
builder.Services.AddSingleton<IConfigService, ConfigService>();

//Messagers service
builder.Services.AddScoped<ISmsService, FakeSmsService>();
builder.Services.AddScoped<IEmailService, MailKit_EmailService>();

////Services of DB
//Db service
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();

//Services of token validator
builder.Services.AddScoped<ITokenValidator, TokenValidator>();

//Authorize and token services
builder.Services.AddScoped<IUserTokenService, UserTokenService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Config Swagger
builder.Services.AddSwaggerGen(c =>
{
    //برای نمایش استرینگی Enum ها
    //c.DescribeAllEnumsAsStrings();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChidoSakht", Version = "v1" });

    //برای نمایش Description کنترلر ها
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebApi.ChidoSakht.xml"), true);

    //For Accept language -- Visible model
    if (Configuration.GetSection("Localization:Visible_AcceptLanguageForSwagger").Get<bool>())
        c.OperationFilter<AcceptLanguageHeaderFilter>();

    //For configure Authentication in swaager Ui
    var security = new OpenApiSecurityScheme
    {
        Name = "JWT Auth",
        Description = "توکن را وارد کنید",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(security.Reference.Id, security);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security , new string[]{ } }
                });
});


//Config JWT Authenfication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtConfig =>
{
    jwtConfig.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = Configuration.GetSection("MainJwtAuthenticationSetting:Issuer").Get<string>(),
        ValidAudience = Configuration.GetSection("MainJwtAuthenticationSetting:Audience").Get<string>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtInfo.SecretKey)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
    jwtConfig.SaveToken = true;
    jwtConfig.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            //Dependent service token validator
            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidator>();

            //Enable it
            return tokenValidatorService.Execute(context);
        }
    };
});


//Service Handler
builder.Services.AddSingleton<HandlerOptions>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Localization cofing
var defaultCulture = Configuration.GetSection("Localization:DefaultCulture").Get<string>() ?? "fa";
var supportedCultures = Configuration.GetSection("Localization:SupportedCultures").Get<string[]>() ?? new string[] { "fa" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture) // زبان پیش‌فرض فارسی
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);


//Swagger config
app.UseSwagger(c =>
{
    c.SerializeAsV2 = false;
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseDeveloperExceptionPage();
}
else
if (app.Environment.IsProduction())
{
    //For takes exeption
    app.UseMiddleware<ExceptionHandling.ExceptionHandlerMiddleware>();
}


app.UseHsts();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

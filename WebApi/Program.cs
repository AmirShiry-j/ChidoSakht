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

var builder = WebApplication.CreateBuilder(args);

IConfiguration Configuration = builder.Configuration;

//Nlog configs
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();


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

//Config service
builder.Services.AddSingleton<IConfigService, ConfigService>();

//Messagers Service
builder.Services.AddScoped<ISmsService, FakeSmsService>();
builder.Services.AddScoped<IEmailService, MailKit_EmailService>();


////Services of DB
//Db service
builder.Services.AddScoped<IDataBaseContext, DataBaseContext>();


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

//Service Handler
builder.Services.AddSingleton<HandlerOptions>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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


app.UseAuthorization();

app.MapControllers();

app.Run();

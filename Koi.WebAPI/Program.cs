using Koi.BusinessObjects;
using Koi.Repositories;
using Koi.Repositories.Models.TestDTO;
using Koi.Services.Hubs;
using Koi.WebAPI.Injection;
using Koi.WebAPI.MiddleWares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// CONFIG FOR JSON PROPERTY API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

//CONFIG FOR JWT AUTHENTICATION ON SWAGGER
builder.Services.AddSwaggerGen(config =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,//Cũ là apikey
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    config.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Koi Farm Shop",
        Description = "Koi Farm Shop API",
        Contact = new OpenApiContact
        {
            Name = "Fanpage",
            Url = new Uri("https://nigga.com")
        },
        License = new OpenApiLicense
        {
            Name = "Front-end URL",
            Url = new Uri("https://nigga.com")
        }
    });

    config.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
});
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//SETUP INJECTION SERVICE
builder.Services.ServicesInjection(builder.Configuration);
//SETUP SERVICE IDENTITY: Allow non alphanumeric
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<KoiFarmShopDbContext>();
//ADD AUTHENTICATION - CONFIG FOR JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});
//ADD AUTHORIZATION
builder.Services.AddAuthorization();
//ADD CORS (IN PROGRESS)

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add SignalR
builder.Services.AddSignalR();

// START - ADD ODATA
var modelBuilder = new ODataConventionModelBuilder();
var edmModel = modelBuilder.GetEdmModel();
modelBuilder.EntityType<OrderTestDTO>();
modelBuilder.EntitySet<KoiFish>("koi-fishes");
modelBuilder.EntitySet<CustomerTestDTO>("Customers");

builder.Services.AddControllers()
    .AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null)
    .AddRouteComponents(
        routePrefix: "api/v1/odata",
        model: edmModel));
// END - ADD ODATA
//ADD CORS
//builder.Services.AddCors(options =>
//{
//    //// Policy allowing any origin, but without AllowCredentials
//    //options.AddPolicy("AllowAnyOrigin",
//    //    policyBuilder =>
//    //    {
//    //        policyBuilder
//    //            .AllowAnyOrigin()
//    //            .AllowAnyHeader()
//    //            .AllowAnyMethod();
//    //        // .AllowCredentials() cannot be used with AllowAnyOrigin
//    //    });

//    // Policy allowing a specific origin with credentials
//    options.AddPolicy("AllowSpecificOrigin",
//        policyBuilder =>
//        {
//            policyBuilder
//                .WithOrigins("https://koifarmshop.netlify.app", "http://localhost:3000/") // Specific origin
//                .AllowAnyHeader()
//                .AllowAnyMethod()
//                .AllowCredentials(); // AllowCredentials works with specific origins
//        });
//});

//CORS - Set Policy
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicyDevelopement", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();

// SCOPE FOR MIGRATION
// explain: The CreateScope method creates a new scope. The scope is a way to manage the lifetime of objects in the container.var scope = app.Services.CreateScope();
var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//CLAIM SERVICE
builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        // Always keep token after reload or refresh browser
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "KOI FARM SHOP API v.01");
        config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
        config.InjectJavascript("/custom-swagger.js");
    });
    try
    {
        app.ApplyMigrations(logger);
    }
    catch (Exception e)
    {
        logger.LogError(e, "An problem occurred during migration!");
    }
}
var context = scope.ServiceProvider.GetRequiredService<KoiFarmShopDbContext>();
try
{
    await DBInitializer.Initialize(context, userManager);
}
catch (Exception e)
{
    logger.LogError(e, "An problem occurred seed data!");
}
// USE CORS
//app.UseCors();
// Use CORS policy
//app.UseCors("AllowSpecificOrigin");
app.UseCors("CorsPolicyDevelopement");

// USE AUTHENTICATION, AUTHORIZATION
app.UseAuthorization();
app.UseAuthentication();

//OTHERS
app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.UseStaticFiles();

// USE MIDDLEWARE
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceTimeMiddleware>();
app.UseMiddleware<UserStatusMiddleware>();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
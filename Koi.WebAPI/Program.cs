using System.Text.Json.Serialization;
using System.Text.Json;
using Koi.WebAPI.Injection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Koi.Repositories.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Koi.Repositories;
using Koi.WebAPI.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// CONFIG FOR JSON PROPERTY API
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower;
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
    //config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        // Always keep token after reload or refresh browser
        config.SwaggerEndpoint("/swagger/v1/swagger.json", "KOI FARM SHOP API v.01");
        config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}
// USE AUTHENTICATION, AUTHORIZATION
app.UseAuthorization();
app.UseAuthentication();
// USE CORS

//OTHERS
app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

// USE MIDDLEWARE
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceTimeMiddleware>();
app.UseMiddleware<UserStatusMiddleware>();

app.Run();
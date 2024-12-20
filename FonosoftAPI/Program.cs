using System.Text;
using CifradoPE.Infraestructura;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Dominio.Validaciones;
using FonosoftAPI.Src.Login.Infraestructura;
using FonosoftAPI.Src.Login.Infraestructura.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region AUTENTICACION
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});
#endregion

#region MYSQL
string? serverMySql = builder.Configuration.GetValue<string>("ConnectionStrings:ServerMysql");
string? userMySql = builder.Configuration.GetValue<string>("ConnectionStrings:UserMysql");
string? databaseMysql = builder.Configuration.GetValue<string>("ConnectionStrings:DatabaseMysql");
string? portMysql = builder.Configuration.GetValue<string>("ConnectionStrings:PortMySql");
string? passwordMysql = builder.Configuration.GetValue<string>("ConnectionStrings:PasswordMysql");
#endregion

#region 
string? host = builder.Configuration.GetValue<string>("Email:host");
int port = builder.Configuration.GetValue<int>("Email:port");
string? usuario = builder.Configuration.GetValue<string>("Email:usuario");
string? contrasenia = builder.Configuration.GetValue<string>("Email:contrasenia");
bool habilitaSSL = builder.Configuration.GetValue<bool>("Email:habilitaSSL");
#endregion 

#region ENCRIPTACION
string key = builder.Configuration.GetValue<string>("Encriptar:Key")!;
string iv = builder.Configuration.GetValue<string>("Encriptar:iv")!;
builder.Services.AddScoped<IAes>(_ => new AesAsimetrico(key, iv));
#endregion

#region LOGINCONTROLLER
builder.Services.AddScoped<IUsuario, Usuario>();
builder.Services.AddScoped<IResponse<IUsuario>, Response<IUsuario>>();
builder.Services.AddScoped<ILoginRepo>(_ => new LoginMysqlRepo(serverMySql!, userMySql!, databaseMysql!, portMysql!, passwordMysql!));
builder.Services.AddScoped<IAuthEmail>(_ => new EmailRepo(host!, port, usuario!, contrasenia!, habilitaSSL));
#endregion

#region VALIDACIONES
builder.Services.AddScoped<ValidarRegistrarUsuario>();
builder.Services.AddScoped<ValidarLoginUsuario>();
builder.Services.AddScoped<ValidarModificarUsuario>();
builder.Services.AddScoped<ValidarResetContrasenia>();
builder.Services.AddScoped<ValidarBuscarUsuarioXId>();

builder.Services.AddScoped<Func<string, IValidar>>(provider => key =>
{
    return key switch
    {
        "ValidarRegistrarUsuario" => provider.GetRequiredService<ValidarRegistrarUsuario>(),
        "ValidarLoginUsuario" => provider.GetRequiredService<ValidarLoginUsuario>(),
        "ValidarModificarUsuario" => provider.GetRequiredService<ValidarModificarUsuario>(),
        "ValidarResetContrasenia" => provider.GetRequiredService<ValidarResetContrasenia>(),
        "ValidarBuscarUsuarioXId" => provider.GetRequiredService<ValidarBuscarUsuarioXId>(),
        _ => throw new ArgumentException("Invalid key", nameof(key))
    };
});
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin() // Permite cualquier origen
                  .AllowAnyHeader() // Permite cualquier encabezado
                  .AllowAnyMethod(); // Permite cualquier método (GET, POST, etc.)
        });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UsePathBase("/fonosoftAPI");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Define la ruta de Swagger UI con el prefijo
    options.SwaggerEndpoint("/fonosoftAPI/swagger/v1/swagger.json", "FonosoftAPI v1");
    options.RoutePrefix = "swagger";  // Prefijo para acceder a Swagger UI
});
app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

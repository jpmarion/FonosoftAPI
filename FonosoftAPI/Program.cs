using CifradoPE.Infraestructura;
using CifradoPE.Infraestructura.Interface;
using FonosoftAPI.Src.Compartido.Interface;
using FonosoftAPI.Src.Compartido.Modelo;
using FonosoftAPI.Src.Login.Dominio.Entidades;
using FonosoftAPI.Src.Login.Dominio.Interface;
using FonosoftAPI.Src.Login.Dominio.Validaciones;
using FonosoftAPI.Src.Login.Infraestructura;
using FonosoftAPI.Src.Login.Infraestructura.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region MYSQL
string? serverMySql = builder.Configuration.GetValue<string>("ConnectionStrings:ServerMysql");
string? userMySql = builder.Configuration.GetValue<string>("ConnectionStrings:UserMysql");
string? databaseMysql = builder.Configuration.GetValue<string>("ConnectionStrings:DatabaseMysql");
string? portMysql = builder.Configuration.GetValue<string>("ConnectionStrings:PortMySql");
string? passwordMysql = builder.Configuration.GetValue<string>("ConnectionStrings:PasswordMysql");
#endregion

#region 
string? host = builder.Configuration.GetValue<string>("host");
int port = builder.Configuration.GetValue<int>("port");
string? usuario = builder.Configuration.GetValue<string>("usuario");
string? contrasenia = builder.Configuration.GetValue<string>("contrasenia");
bool habilitaSSL = builder.Configuration.GetValue<bool>("habilitaSSL");
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

builder.Services.AddScoped<Func<string, IValidar>>(provider => key =>
{
    return key switch
    {
        "ValidarRegistrarUsuario" => provider.GetRequiredService<ValidarRegistrarUsuario>(),
        _ => throw new ArgumentException("Invalid key", nameof(key))
    };
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

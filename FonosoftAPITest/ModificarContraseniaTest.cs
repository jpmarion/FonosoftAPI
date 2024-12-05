using System.Net;
using System.Text;
using System.Text.Json;

namespace FonosoftAPITest
{
    public class ModificarContraseniaTest
    {
        private HttpClient? _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5156/api/Login/")
            };
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task NombreUsuarioVacio()
        {
            var body = new
            {
                nombreUsuario = "",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = "$123456Jpm"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task NombreUsuarioInexistente()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = "$123456Jpm"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    
        [Test]
        public async Task ContraseniaVacia()
        {
            var body = new
            {
                nombreUsuario = "jpmarion",
                contrasenia = "",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = "$123456Jpm"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    
        [Test]
        public async Task NuevaContraseniaVacia()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "",
                repetirContrasenia = "$123456Jpm"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

            [Test]
        public async Task RepetirContraseniaVacia()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = ""
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    
        [Test]
        public async Task ContraseniasDiferente()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = "$123456Jp"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

            [Test]
        public async Task ModificarContrasenia()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "Jpm123456$",
                nuevaContrasenia = "$123456Jpm",
                repetirContrasenia = "$123456Jpm"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ModificarContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
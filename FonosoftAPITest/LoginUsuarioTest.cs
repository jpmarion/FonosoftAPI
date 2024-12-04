using System.Net;
using System.Text;
using System.Text.Json;

namespace FonosoftAPITest
{
    public class LoginUsuarioTest
    {
        private HttpClient? _httpClient;
        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5156/api/")
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
                contrasenia = "ErhnckrW"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("Login", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ContraseniaVacia()
        {
            var body = new
            {
                nombreUsuario = "jpmarion",
                contrasenia = ""
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("Login", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task NombreUsuarioInexistente()
        {
            var body = new
            {
                nombreUsuario = "toto",
                contrasenia = "ErhnckrW"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("Login", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Login()
        {
            var body = new
            {
                nombreUsuario = "jpmarion",
                contrasenia = "ErhnckrW"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("Login", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
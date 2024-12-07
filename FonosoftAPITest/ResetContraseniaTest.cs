using System.Net;
using System.Text;
using System.Text.Json;

namespace FonosoftAPITest
{

    public class ResetContraseniaTest
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
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ResetContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task NombreUsuarioInexistente()
        {
            var body = new
            {
                nombreUsuario = "sdfsdfds",
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ResetContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task ResetContrasenia()
        {
            var body = new
            {
                nombreUsuario = "jpmarion",
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PatchAsync("ResetContrasenia", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
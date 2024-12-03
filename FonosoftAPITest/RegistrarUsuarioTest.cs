using System.Net;
using System.Text;
using System.Text.Json;

namespace FonosoftAPITest
{
    public class RegistrarUsuarioTest
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
        public async Task RegistrarUsuarioNombreUsuarioVacio()
        {
            var body = new
            {
                nombreUsuario = "",
                email = Faker.Internet.Email()
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("RegistrarUsuario", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RegistrarUsuarioEmailVacio()
        {
            var body = new
            {
                nombreUsuario = Faker.Internet.UserName(),
                email = ""
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("RegistrarUsuario", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RegistrarUsuarioEmailNoValido()
        {
            var body = new
            {
                nombreUsuario = Faker.Internet.UserName(),
                email = "ssldfwpe232fwe"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("RegistrarUsuario", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task RegistrarUsuario(){
            var body = new
            {
                nombreUsuario = Faker.Internet.UserName(),
                email = "Abc123456$"
            };

            var contenido = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient!.PostAsync("RegistrarUsuario", contenido);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
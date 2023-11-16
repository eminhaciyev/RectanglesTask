using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RectanglesTask.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RectanglesTask.Controllers.Tests
{
    [TestClass()]
    public class RectangleControllerTests : IClassFixture<WebApplicationFactory<RectanglesTask.Program>>
    {
        private readonly HttpClient _client;

        public RectangleControllerTests(WebApplicationFactory<RectanglesTask.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task FindRectangles_ReturnsSuccessStatusCode()
        {
            var coordinates = new List<int[]> { new[] { 1, 1 } };
            var content = new StringContent(JsonConvert.SerializeObject(coordinates), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/getCoordinates", content);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task FindRectangles_WithInvalidInput_ReturnsBadRequest()
        {
            var invalidContent = new StringContent("", Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/getCoordinates", invalidContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
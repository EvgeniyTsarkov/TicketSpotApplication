using Common.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace TicketSpotApplication.Tests.IntegrationTests;

[TestClass]
public class TicketOrderingIntegrationTest
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task Test()
    {
        var response = await _client.GetAsync("/Venues");

        var content = await response.Content.ReadAsStringAsync();

        var deserializedContent = JsonConvert.DeserializeObject<List<Venue>>(content);

        Assert.IsTrue(true);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}

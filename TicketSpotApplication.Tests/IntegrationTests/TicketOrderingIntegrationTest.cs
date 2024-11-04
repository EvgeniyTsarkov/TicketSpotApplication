using Microsoft.AspNetCore.Mvc.Testing;
using PublicWebAPI.Business.Dtos;
using TicketSpotApplication.Tests.Helpers;

namespace TicketSpotApplication.Tests.IntegrationTests;

[TestClass]
public class TicketOrderingIntegrationTest
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    private readonly Guid _cartId = new Guid("0a1b428a-9fb0-4ff2-90ef-d3d720304cc0");

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task OrdersShallBePlacedCorrectly()
    {
        var orderPayload = new OrderPayloadDto
        {
            EventId = 1,
            SeatId = 1,
            PriceOptionId = 1
        };

        var httpContent = TestHelper.BuildHttpContent(orderPayload);

        var response = await _client.PostAsync($"/orders/carts/{_cartId}", httpContent);

        //var response = await _client.GetAsync("/Venues");

        //var content = await response.Content.ReadAsStringAsync();

        //var deserializedContent = JsonConvert.DeserializeObject<List<Venue>>(content);

        //Assert.IsTrue(true);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}

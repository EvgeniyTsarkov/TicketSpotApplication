using Newtonsoft.Json;
using System.Text;

namespace TicketSpotApplication.Tests.Helpers;

public class TestHelper
{
    public static HttpContent BuildHttpContent<TEntity>(TEntity entity) where TEntity : class
    {
        var jsonPayload = JsonConvert.SerializeObject(entity);

        return new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    }
}

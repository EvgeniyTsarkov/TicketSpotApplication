using System.ComponentModel.DataAnnotations;

namespace PublicWebAPI.Configuration;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public AppSettings GetAppSettings(IConfiguration configuration) =>
        new()
        {
            ConnectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>(),
        };
}

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}

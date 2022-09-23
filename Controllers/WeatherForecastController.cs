using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Text;

namespace UptimeMonitorApi.Controllers;

[ApiController]
[Route("api")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet()]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("{ip}")]
    public string Ping(string ip)
    {
        bool pingable = false;
        Ping pinger = null;

        try
        {
            pinger = new Ping();
            PingReply reply = pinger.Send(ip);
            pingable = reply.Status == IPStatus.Success;
        }
        catch (PingException ex)
        {
            return ex.GetBaseException().Message;
        }
        finally
        {
            if (pinger != null)
            {
                pinger.Dispose();
            }
        }

        return "true";
    }


}

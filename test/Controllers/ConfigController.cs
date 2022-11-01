using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace test.Controllers
{
    [ApiController]
    [Route("Config")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet("add")]
        public List<object> AddService([FromQuery] string userId, string service)
        {
            _logger.LogInformation("Adding new user. uId: {0}. service: {1}", userId, service);
            var ans = FileMaster.AddUser(userId, service);
            if (ans.Count == 0) _logger.LogInformation("This service doesn't exist");
            else _logger.LogInformation("Success");
            
            return ans;
        }
        [HttpGet("del")]
        public string DelService(string userId, string service)
        {
            _logger.LogInformation("Deleting user. uID: {0} service: {1}", userId, service);
            var ans = FileMaster.DelUser(userId, service);
            _logger.LogInformation("Answer from api: {0}", ans);
            return ans;
        }
        [HttpGet]
        public List<object> Get([FromQuery]string service)
        {
            var conf = FileMaster.GetConfig(service);
            _logger.LogInformation("GET: {0}. Answer: {1}", service, conf.service);
            return conf.data;
        }

        [HttpPost]
        public string Post([FromBody]JsonElement value)
        {
            _logger.LogInformation("POST: {0}", value);
            var result = FileMaster.SaveConfig(value); // 0 - already exist; 1 - saved; 2 - format error
            switch (result)
            {
                case 0:
                    _logger.LogInformation("Config with this name already exist");
                    return ("Config with this already exist. Please, use PUT to update");
                case 1:
                    _logger.LogInformation("New config saved");
                    return ("Config saved successfully");
                case 2:
                    _logger.LogInformation("Unknown json format");
                    return ("Unknown json format");
                default:
                    _logger.LogWarning("Unknown error");
                    return "Unknown error";
            }
        }
        [HttpPut]
        public string Put([FromBody]JsonElement value)
        {
            _logger.LogInformation("PUT: {0}", value);
            var result = FileMaster.UpdateConfig(value); // 0 - not exist; 1 - updated; 2 - format error
            switch (result.Item1)
            {
                case 0:
                    _logger.LogInformation("Config with this name don't exist");
                    return "Config with this name don't exist. Please, use POST to create new one";
                case 1:
                    _logger.LogInformation("Updated config saved as {0}", result.Item2);
                    return ("Config saved as new version and named " + result.Item2);
                case 2:
                    _logger.LogInformation("Unknown json format");
                    return "Unknown json format";
                default:
                    _logger.LogWarning("Unknown error");
                    return "Unknown error";
            }
        }
        [HttpDelete]
        public string Delete([FromQuery] string service)
        {
            _logger.LogInformation("DELETE: {0}", service);
            var result = FileMaster.DeleteConfig(service); // 0 - don't exist; 1 - using by someone; 2 - deleted
            switch (result)
            {
                case 0:
                    _logger.LogInformation("Config not found");
                    return "Config not found";
                case 1:
                    _logger.LogInformation("Not deleted. This config is being used by someone");
                    return "Not deleted. Someone is using it";
                case 2:
                    _logger.LogInformation("Deleted");
                    return "Deleted";
                default:
                    _logger.LogWarning("Unknown error");
                    return "Unknown error";
            }
        }
    }
}

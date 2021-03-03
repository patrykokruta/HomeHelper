using HomeHelper.Common.Enums;
using HomeHelper.SDK;
using HomeHelper.SDK.HttpControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeHelper.Sonoff
{
    public class SonoffBasicR3 : ISwitch
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public SonoffBasicR3()
        {

        }
        public SonoffBasicR3(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string Name => "SonoffBasicR3";
        public ProtocolType Protocol => ProtocolType.Http;
        public DeviceType Type => DeviceType.Switch;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IDevice, SonoffBasicR3>();
            services.AddScoped<ISwitch, SonoffBasicR3>();
        }

        public async Task<bool?> IsOn(string ip, int port)
        {
            UriBuilder builder = new UriBuilder()
            {
                Scheme = "http",
                Host = ip,
                Port = port,
                Path = "zeroconf/info"
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(new BodyContent() { Deviceid = "", Data = new Data() }, _jsonSerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(builder.Uri.ToString(), data);
            var jsonResponse = await response.Content.ReadFromJsonAsync<BodyContent>(_jsonSerializerOptions);

            if (jsonResponse.Error == 0 && response.IsSuccessStatusCode)
            {
                if (jsonResponse.Data.Switch == "on" || jsonResponse.Data.Switch == "On") return true;
                else return false;
            }
            return null;
        }

        public async Task<int?> SignalStrength(string ip, int port)
        {
            UriBuilder builder = new UriBuilder()
            {
                Scheme = "http",
                Host = ip,
                Port = port,
                Path = "zeroconf/signal_strength"
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(new BodyContent() { Deviceid = "", Data = new Data() }, _jsonSerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(builder.Uri.ToString(), data);
            var jsonResponse = await response.Content.ReadFromJsonAsync<BodyContent>(_jsonSerializerOptions);

            if (jsonResponse.Error == 0 && response.IsSuccessStatusCode)
            {
                return jsonResponse.Data.SignalStrength;
            }
            return null;
        }

        public async Task<bool> TurnOff(string ip, int port)
        {
            UriBuilder builder = new UriBuilder()
            {
                Scheme = "http",
                Host = ip,
                Port = port,
                Path = "zeroconf/switch"
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(new BodyContent() { Deviceid = "", Data = new Data() { Switch = "off" } }, _jsonSerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(builder.Uri.ToString(), data);
            var jsonResponse = await response.Content.ReadFromJsonAsync<BodyContent>(_jsonSerializerOptions);

            if (jsonResponse.Error == 0 && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> TurnOn(string ip, int port)
        {
            UriBuilder builder = new UriBuilder()
            {
                Scheme = "http",
                Host = ip,
                Port = port,
                Path = "zeroconf/switch"
            };

            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(new BodyContent() { Deviceid = "", Data = new Data() { Switch = "on" } }, _jsonSerializerOptions);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(builder.Uri.ToString(), data);
            var jsonResponse = await response.Content.ReadFromJsonAsync<BodyContent>(_jsonSerializerOptions);

            if (jsonResponse.Error == 0 && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public class BodyContent
        {
            public string Deviceid { get; set; }
            public int? Seq { get; set; }
            public int? Error { get; set; }
            public Data Data { get; set; }
        }
        public class Data
        {
            public string Switch { get; set; }
            public string Startup { get; set; }
            public string Pulse { get; set; }
            public int? PulseWidth { get; set; }
            public int? SignalStrength { get; set; }
        }
    }
}

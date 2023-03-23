using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.UI.Input;
using JsonConstructorAttribute = Newtonsoft.Json.JsonConstructorAttribute;

namespace fitness_game.Api
{
    public class Cube {

        public Cube() { }

        [JsonProperty(PropertyName="id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cube_id")]
        public int CubeId { get; set; }

        [JsonProperty(PropertyName = "pose")]
        public string Pose { get; set; }
    }

    public class Tap {
        public Tap() { }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "cube_id")]
        public int CubeId { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public float Timestamp { get; set; }
    }

    public class Sequence {
        public Sequence() { }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "cubes")]
        public Cube[] Cubes { get; set; }

        [JsonProperty(PropertyName = "taps")]
        public Tap[] Taps
        {
            get; set;
        }
    }

    public interface IApiV1Client
    {
        Task<Sequence []> GetSequences();
        Task<Sequence> CreateSequence(Sequence sequence);
    }

    class ApiV1Client : IApiV1Client, IDisposable
    {
        readonly RestClient _client;

        public ApiV1Client()
        {
            var options = new RestClientOptions("http://localhost:8000/api/v1/");
            _client = new RestClient(options);
        }

        public async Task<Sequence> CreateSequence(Sequence sequence)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<Sequence[]> GetSequences()
        {
            var response = await _client.GetJsonAsync<Sequence []>("sequences");
            return response;
        }
    }
}

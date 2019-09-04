using Newtonsoft.Json;

namespace Services.Hydra.WebApi.Models
{
    public abstract class Entity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        public abstract string TypeName { get; }
    }
}

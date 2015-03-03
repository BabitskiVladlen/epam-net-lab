using Newtonsoft.Json;

namespace RpR.JsonEntities
{
    public class CoreJsonEntity
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

}
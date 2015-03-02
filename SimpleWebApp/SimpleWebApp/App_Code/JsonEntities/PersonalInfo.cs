using Newtonsoft.Json;

namespace SimpleWebApp.JsonEntities
{
    public class PersonalInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public Surname Surname { get; set; }

        [JsonProperty(PropertyName = "phones")]
        public string[] Phones { get; set; }
    }
    public class Surname
    {
        [JsonProperty(PropertyName = "surname1")]
        public string Surname1 { get; set; }

        [JsonProperty(PropertyName = "surname2")]
        public string Surname2 { get; set; }
    }
}
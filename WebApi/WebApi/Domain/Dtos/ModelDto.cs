using System.Text.Json.Serialization;

namespace WebApi.Domain.Dtos
{
    public class ModelDto
    {
        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("codigo")]
        public int Code { get; set; }
    }
}

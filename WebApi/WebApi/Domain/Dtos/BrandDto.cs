using System.Text.Json.Serialization;

namespace WebApi.Domain.Dtos
{
    public class BrandDto
    {
        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("codigo")]
        public string Code { get; set; }
    }
}

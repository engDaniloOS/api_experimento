using System.Text.Json.Serialization;

namespace WebApi.Domain.Dtos
{
    public class CarModelDto: BaseDto
    {
        [JsonPropertyName("modelos")]
        public List<ModelDto> Models { get; set; }
    }
}

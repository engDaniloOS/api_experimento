using System.Text.Json.Serialization;

namespace WebApi.Domain.Dtos
{
    public abstract class BaseDto
    {
        [JsonIgnore]
        public bool HasError { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }
    }
}

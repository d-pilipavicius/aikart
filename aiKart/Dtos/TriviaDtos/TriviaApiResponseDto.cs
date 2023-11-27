using Newtonsoft.Json;

namespace aiKart.Dtos.TriviaDtos;

public class TriviaApiResponseDto
{
    [JsonProperty("response_code")]
    public int ResponseCode { get; set; }

    [JsonProperty("results")]
    public List<TriviaQuestionDto> Results { get; set; }
}
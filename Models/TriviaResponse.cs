using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class TriviaResponse
    {
        [JsonPropertyName("response_code")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("results")]
        public List<QuizQuestion>? Results { get; set; }
    }
}

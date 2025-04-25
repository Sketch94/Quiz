using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class QuizQuestion
    {
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("difficulty")]
        public string? Difficulty { get; set; }

        [JsonPropertyName("question")]
        public string? Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public string? CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public List<string>? IncorrectAnswers { get; set; }
    }
}

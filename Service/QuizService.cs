using System.Net.Http;
using System.Text.Json;
using QuizApp.Models;
using System.Threading.Tasks;
using QuizApp.Extensions;

namespace QuizApp.Service
{
    public class QuizService
    {
        private readonly HttpClient _httpClient;

        public QuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<QuizQuestion>> GetQuizQuestionsAsync(int amount = 5)
        {
            string url = $"https://opentdb.com/api.php?amount={amount}&type=multiple";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                TriviaResponse triviaResponse = await JsonSerializer.DeserializeAsync<TriviaResponse>(stream, options);
                return triviaResponse.Results;
            }

            return new List<QuizQuestion>();
        }
    }
}

namespace QuizApp.Models
{
    public class QuestionViewModel
    {
        public int Index { get; set; }
        public string? Question { get; set; }
        public List<string>? Answers { get; set; }
    }
}

namespace QuizApp.Models
{
    public class ResultViewModel
    {
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public List<QuizQuestion> IncorrectQuestions { get; set; } = new List<QuizQuestion>();

    }
}

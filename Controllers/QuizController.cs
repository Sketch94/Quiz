using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using QuizApp.Service;
using QuizApp.Extensions;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizService _quizService;
        private const string SessionQuestionsKey = "QuizQuestions";
        private const string SessionScoreKey = "QuizScore";
        private const string SessionIncorrectQuestionsKey = "IncorrectQuestions";

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        // Startsida
        public IActionResult Index()
        {
            return View();
        }

        // Starta quizet: Hämta frågor och spara i session
        [HttpPost]
        public async Task<IActionResult> Start()
        {
            var questions = await _quizService.GetQuizQuestionsAsync();
            HttpContext.Session.SetInt32(SessionScoreKey, 0);
            HttpContext.Session.SetObject(SessionQuestionsKey, questions);
            HttpContext.Session.SetObject(SessionIncorrectQuestionsKey, new List<QuizQuestion>()); // Återställ lista över felaktiga frågor
            return RedirectToAction("Question", new { index = 0 });
        }

        // Visa aktuell fråga
        public IActionResult Question(int index)
        {
            var questions = HttpContext.Session.GetObject<List<QuizQuestion>>(SessionQuestionsKey);
            if (questions == null || index >= questions.Count)
            {
                return RedirectToAction("Result");
            }

            var currentQuestion = questions[index];

            // Kombinera rätt och felaktiga svar
            var answers = new List<string>(currentQuestion.IncorrectAnswers);
            answers.Add(currentQuestion.CorrectAnswer);
            answers = answers.OrderBy(a => Guid.NewGuid()).ToList();

            var viewModel = new QuestionViewModel
            {
                Index = index,
                Question = currentQuestion.Question,
                Answers = answers
            };

            return View(viewModel);
        }

        // Hantera användarens svar
        [HttpPost]
        public IActionResult Answer(int index, string selectedAnswer)
        {
            var questions = HttpContext.Session.GetObject<List<QuizQuestion>>(SessionQuestionsKey);
            var incorrectQuestions = HttpContext.Session.GetObject<List<QuizQuestion>>(SessionIncorrectQuestionsKey) ?? new List<QuizQuestion>();

            if (questions == null || index >= questions.Count)
            {
                return RedirectToAction("Result");
            }

            var currentQuestion = questions[index];
            int score = HttpContext.Session.GetInt32(SessionScoreKey) ?? 0;

            if (selectedAnswer == currentQuestion.CorrectAnswer)
            {
                score++;
                HttpContext.Session.SetInt32(SessionScoreKey, score);
            }
            else
            {
                incorrectQuestions.Add(currentQuestion); // Spara felaktigt besvarad fråga
                HttpContext.Session.SetObject(SessionIncorrectQuestionsKey, incorrectQuestions);
            }

            index++;
            if (index >= questions.Count)
            {
                return RedirectToAction("Result");
            }

            return RedirectToAction("Question", new { index });
        }

        // Visa slutresultatet
        public IActionResult Result()
        {
            int score = HttpContext.Session.GetInt32(SessionScoreKey) ?? 0;
            var questions = HttpContext.Session.GetObject<List<QuizQuestion>>(SessionQuestionsKey);
            var incorrectQuestions = HttpContext.Session.GetObject<List<QuizQuestion>>(SessionIncorrectQuestionsKey) ?? new List<QuizQuestion>();

            var model = new ResultViewModel
            {
                Score = score,
                TotalQuestions = questions?.Count ?? 0,
                IncorrectQuestions = incorrectQuestions
            };

            return View(model);
        }
    }
}

using Calculator.Interfaces;
using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMathExpressionCalculate _mathExpressionCalculate;
        private readonly IMathExpressionParser _mathExpressionParser;
        private readonly ResultManager _resultManager;

        public HomeController(ILogger<HomeController> logger, IMathExpressionCalculate mathExpressionCalculate, IMathExpressionParser mathExpressionParser, ResultManager resultManager)
        {
            _logger = logger;
            _mathExpressionCalculate = mathExpressionCalculate;
            _mathExpressionParser = mathExpressionParser;
            _resultManager = resultManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new MathExpressionModel());
        }

        [HttpPost]
        public IActionResult Index(MathExpressionModel model)
        {

            if (ModelState.IsValid)
            {
                var resultParse = _mathExpressionParser.Parse(model.Expression);
                var resultCalculate = _mathExpressionCalculate.Evaluate(resultParse);

                var resultsJson = HttpContext.Session.GetString("Results");
                var results = string.IsNullOrEmpty(resultsJson)
                 ? new List<string>()
                 : JsonSerializer.Deserialize<List<string>>(resultsJson);

                results.Add($"{model.Expression} = {resultCalculate}");

                HttpContext.Session.SetString("Results", JsonSerializer.Serialize(results));

                ViewBag.Results = results;
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

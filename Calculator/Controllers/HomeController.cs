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
        private readonly IMathService _mathService;
        private readonly SessionResultHistory _sessionHistoryResult;
        private readonly ResultManager _resultManager;

        public HomeController(ILogger<HomeController> logger, IMathService mathService, ResultManager resultManager, SessionResultHistory sessionHistoryResult)
        {
            _logger = logger;
            _mathService = mathService;
            _resultManager = resultManager;
            _sessionHistoryResult = sessionHistoryResult;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var results = _resultManager.GetResults();

            ViewBag.Results = results.Select(r => $"{r.Expression} = {r.ExpressionResult}");

            return View(new MathExpressionModel());
        }

        [HttpPost]
        public IActionResult Index(MathExpressionModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            model.ImmutableExpression = model.Expression;

            double calculateResult = _mathService.Calculator(model.Expression);

            _sessionHistoryResult.AddResult(model.ImmutableExpression, calculateResult);

            return RedirectToAction("Index");
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

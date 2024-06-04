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
        private readonly IResultsHistory _historyResult;

        public HomeController(ILogger<HomeController> logger, IMathService mathService, IResultsHistory historyResult)
        {
            _logger = logger;
            _mathService = mathService;
            _historyResult = historyResult;
        }

        [HttpGet]
        public IActionResult Index()
        {
            TransferCalculationHistory();

            return View(new MathExpressionModel());
        }

        [HttpPost]
        public IActionResult Index(MathExpressionModel model)
        {
            if (!ModelState.IsValid)
            {
                TransferCalculationHistory();

                return View(model);
            }

            double calculateResult = _mathService.Calculate(model.Expression);

            _historyResult.AddResult(model.Expression, calculateResult);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void TransferCalculationHistory()
        {
            var results = _historyResult.GetResults();

            ViewBag.Results = results;
        }
    }
}

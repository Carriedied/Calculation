using Calculator.Interfaces;
using System.Text.Json;

namespace Calculator.Models
{
    public class SessionResultHistory
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ResultManager _resultManager;

        public SessionResultHistory(IHttpContextAccessor httpContextAccessor, ResultManager resultManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _resultManager = resultManager;
        }

        public void AddResult(string expression, double expressionResult)
        {
            Results results = new Results(expression, expressionResult);

            _resultManager.AddResult(results);

            var session = _httpContextAccessor.HttpContext.Session;

            session.SetString("Results", JsonSerializer.Serialize(_resultManager.GetResults()));
        }

        public IEnumerable<Results> GetResults()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var resultsJson = session.GetString("Results");

            if (string.IsNullOrEmpty(resultsJson))
            {
                return Enumerable.Empty<Results>();
            }
            else
            {
                return JsonSerializer.Deserialize<IEnumerable<Results>>(resultsJson);
            }
        }
    }
}

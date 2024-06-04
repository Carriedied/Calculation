using Calculator.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calculator.Models
{
    public class SessionResultHistory : IResultsHistory
    {
        private readonly ISession _session;

        public SessionResultHistory(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddResult(string expression, double expressionResult)
        {
            var results = GetResults().ToList();
            results.Add(new ExpressionsResults(expression, expressionResult));

            _session.SetString("Results", JsonSerializer.Serialize(results));
        }

        public IEnumerable<ExpressionsResults> GetResults()
        {
            var jsonResults = _session.GetString("Results");

            if (string.IsNullOrEmpty(jsonResults))
            {
                return Enumerable.Empty<ExpressionsResults>();
            }

            return JsonSerializer.Deserialize<IEnumerable<ExpressionsResults>>(jsonResults);
        }
    }
}

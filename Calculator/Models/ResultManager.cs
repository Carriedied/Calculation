using Calculator.Interfaces;

namespace Calculator.Models
{
    public class ResultManager
    {
        private List<Results> _results;

        public ResultManager()
        {
            _results = new List<Results>();
        }

        public void AddResult(Results result)
        {
            _results.Add(result);
        }

        public IEnumerable<Results> GetResults()
        {
            return _results.ToArray();
        }
    }
}

namespace Calculator.Interfaces
{
    public interface IResultsHistory
    {
        void AddResult(string expression, double result);
        IEnumerable<Models.ExpressionsResults> GetResults();
    }
}

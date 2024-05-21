namespace Calculator.Models
{
    public class Results
    {
        public Results(string expression, double expressionResult)
        {
            Expression = expression;
            ExpressionResult = expressionResult;
        }

        public string Expression { get; set; }
        public double ExpressionResult { get; set; }
    }
}

using Calculator.Interfaces;

namespace Calculator.Services
{
    public class MathService : IMathService
    {
        private IMathExpressionParser _parser;
        private IMathExpressionCalculate _calculate;

        public MathService(IMathExpressionParser parser, IMathExpressionCalculate calculate)
        {
            _parser = parser;
            _calculate = calculate;
        }

        public double Calculator(string expression)
        {
            string parserResult = _parser.Parse(expression);
            double calculate = _calculate.Evaluate(parserResult);

            return calculate;
        }
    }
}

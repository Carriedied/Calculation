using Calculator.Interfaces;

namespace Calculator.Services
{
    public class MathService : IMathService
    {
        private IParser _parser;
        private ICalculator _calculator;

        public MathService(IParser parser, ICalculator calculator)
        {
            _parser = parser;
            _calculator = calculator;
        }

        public double Calculate(string expression)
        {
            string parserResult = _parser.Parse(expression);
            double calculatorResult = _calculator.Evaluate(parserResult);

            return calculatorResult;
        }
    }
}

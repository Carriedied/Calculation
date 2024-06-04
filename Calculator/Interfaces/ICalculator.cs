using Calculator.Services;

namespace Calculator.Interfaces
{
    public interface ICalculator
    {
        double Evaluate(string postfixExpression);
    }
}

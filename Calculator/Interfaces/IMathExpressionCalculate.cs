using Calculator.Services;

namespace Calculator.Interfaces
{
    public interface IMathExpressionCalculate
    {
        double Evaluate(string postfixExpression);
    }
}

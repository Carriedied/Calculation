using Calculator.Interfaces;
using Calculator.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace Calculator.Services
{
    public class MathExpressionCalculate : IMathExpressionCalculate
    {
        public double Evaluate(string expression)
        {
            SignsMathExpression tokens = new SignsMathExpression();
            Stack<double> numbers = new Stack<double>();

            char sign;
            double last;
            string number;
            double first;
            double second;

            for (int i = 0; i < expression.Length; i++)
            {
                sign = expression[i];

                if (char.IsDigit(sign))
                {
                    number = tokens.GetStringNumber(expression, ref i);

                    numbers.Push(Convert.ToDouble(number));
                }
                else if (tokens.IsThereSignInListOperators(sign))
                {
                    if (sign == SignsMathExpression.UnaryMinus)
                    {
                        if (numbers.Count > 0)
                        {
                            last = numbers.Pop();
                        }
                        else
                        {
                            last = 0;
                        }

                        numbers.Push(Execute(0, last, SignsMathExpression.Minus));

                        continue;
                    }

                    if (numbers.Count > 0)
                    {
                        second = numbers.Pop();
                    }
                    else
                    {
                        second = 0;
                    }

                    if (numbers.Count > 0)
                    {
                        first = numbers.Pop();
                    }
                    else
                    {
                        first = 0;
                    }

                    numbers.Push(Execute(first, second, sign));
                }
            }

            return numbers.Pop();
        }

        private double Execute(double firstNumber, double secondNumber, char sign)
        {
            switch (sign)
            {
                case SignsMathExpression.Plus:
                    return firstNumber + secondNumber;

                case SignsMathExpression.Minus:
                    return firstNumber - secondNumber;

                case SignsMathExpression.Multiply:
                    return firstNumber * secondNumber;

                case SignsMathExpression.Divide:
                    return firstNumber / secondNumber;

                default:
                    return 0;
            }
        }
    }
}

using Calculator.Interfaces;
using Calculator.Models;
using System.Linq.Expressions;

namespace Calculator.Services
{
    public class Parser : IMathExpressionParser
    {
        public string Parse(string expression)
        {
            string postfixExpression = ToPostfix(expression);

            return postfixExpression;
        }

        private string ToPostfix(string infixExpression)
        {
            Stack<char> symbols = new Stack<char>();
            SignsMathExpression tokens = new SignsMathExpression();

            string postfixExpression = "";
            char operation;
            char element;

            for (int i = 0; i < infixExpression.Length; i++)
            {
                element = infixExpression[i];

                if (char.IsDigit(element))
                {
                    postfixExpression += tokens.GetStringNumber(infixExpression, ref i) + " ";
                }
                else if (element == SignsMathExpression.OpenBracket)
                {
                    symbols.Push(element);
                }
                else if (element == SignsMathExpression.CloseBracket)
                {
                    while (symbols.Count > 0 && symbols.Peek() != SignsMathExpression.OpenBracket)
                    {
                        postfixExpression += symbols.Pop();
                    }

                    symbols.Pop();
                }
                else if (tokens.IsThereSignInListOperators(element))
                {
                    operation = element;

                    if (operation == SignsMathExpression.Minus && (i == 0 || (i > 1 && tokens.IsThereSignInListOperators(infixExpression[i - 1]))))
                    {
                        operation = SignsMathExpression.UnaryMinus;
                    }

                    while (symbols.Count > 0 && tokens.IsSavedCharacterPriorityAnotherCharacter(symbols.Peek(), operation))
                    {
                        postfixExpression += symbols.Pop();
                    }

                    symbols.Push(operation);
                }
            }

            foreach (char signs in symbols)
            {
                postfixExpression += signs;
            }

            return postfixExpression;
        }
    }
}

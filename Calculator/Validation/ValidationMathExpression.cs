using Calculator.Models;
using System.ComponentModel.DataAnnotations;

namespace Calculator.Validation
{
    public class ValidationMathExpression : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? inputExpression = value as string;

            if (InputNotValid(inputExpression))
            {
                return new ValidationResult("Вы ввели неверное выражение.");
            }

            string replacedInputExpression = inputExpression.Replace(" ", "");

            if (NotFloatingNumber(replacedInputExpression, SignsMathExpression.FloatPoint))
            {
                return new ValidationResult("Вы ввели неверное число с десятичной дробью или поставили знак \",\", вместо \".\".");
            }
            
            if (CharactersNotValid(replacedInputExpression, SignsMathExpression.AllSigns))
            {
                return new ValidationResult("Выражение содержит недопустимые символы.");
            }

            if (BracketsBalanceNotValid(replacedInputExpression, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket))
            {
                return new ValidationResult("Неправильно расставлены скобки.");
            }

            if (IsDivisionZero(replacedInputExpression, SignsMathExpression.Divide))
            {
                return new ValidationResult("Нельзя делить на ноль.");
            }

            if (SignPlacedNotCorrectly(replacedInputExpression, SignsMathExpression.AllSigns, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket, SignsMathExpression.Minus))
            {
                return new ValidationResult("В выражении неверно расставлены знаки или числа.");
            }

            if (replacedInputExpression.Any(c => SignsMathExpression.Brackets.Contains(c)))
            {
                if (ExpressionParenthesesNotCorrect(replacedInputExpression, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket, SignsMathExpression.AllSigns, SignsMathExpression.Minus, SignsMathExpression.FloatPoint))
                {
                    return new ValidationResult("Выражение в скобках некорректно.");
                }
            }

            return ValidationResult.Success;
        }

        private bool InputNotValid(string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        private bool NotFloatingNumber(string input, char floatPoint)
        {
            bool isNumber = false;
            int floatPointCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    isNumber = true;
                }
                else if (input[i] == floatPoint && isNumber)
                {
                    floatPointCount++;

                    if (floatPointCount > 1)
                    {
                        return true;
                    }
                }
                else
                {
                    isNumber = false;
                    floatPointCount = 0;
                }
            }

            return false;
        }

        private bool CharactersNotValid(string input, string allSymbols)
        {
            return input.All(c => char.IsDigit(c) || allSymbols.Contains(c)) ? false : true;
        }

        private bool BracketsBalanceNotValid(string input, char openBracket, char closeBracket)
        {
            int balanceOfParentheses = 0;

            foreach (char sign in input)
            {
                if (sign == openBracket)
                {
                    balanceOfParentheses++;
                }
                else if (sign == closeBracket)
                {
                    balanceOfParentheses--;
                }

                if (balanceOfParentheses < 0)
                {
                    return true;
                }
            }

            return balanceOfParentheses == 0 ? false : true;
        }

        private bool IsDivisionZero(string input, char signDivide)
        {
            int nextElement = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (i + nextElement < input.Length)
                {
                    if (input[i] == signDivide && input[i + nextElement] == '0')
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool SignPlacedNotCorrectly(string input, string allSigns, char openBracket, char closeBracket, char minus)
        {
            int startIndex = 0;
            int nextIndex = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (allSigns.Contains(input[startIndex]))
                {
                    if (input[startIndex] != minus && input[startIndex] != openBracket)
                    {
                        return true;
                    }
                }
                
                if (i + nextIndex == input.Length)
                {
                    if (allSigns.Contains(input[i]) && input[i] != closeBracket)
                    {
                        return true;
                    }
                }

                if (input[i] == openBracket && allSigns.Contains(input[i + nextIndex]) == true)
                {
                    if (input[i + nextIndex] != minus && input[i + nextIndex] != openBracket)
                    {
                        return true;
                    }
                }
                
                if (i + nextIndex < input.Length)
                {
                    if (input[i] == closeBracket && allSigns.Contains(input[i + nextIndex]) == false)
                    {
                        return true;
                    }
                }
                
                if (i + nextIndex < input.Length)
                {
                    if (SignsMathExpression.AllOperations.Contains(input[i]) && SignsMathExpression.AllOperations.Contains(input[i + nextIndex]))
                    {
                        if (input[i + nextIndex] != minus || input[i + nextIndex] != closeBracket)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool ExpressionParenthesesNotCorrect(string input, char openBracket, char closeBracket, string allSigns, char minus, char floatPoint)
        {
            int indexLastOpenBracket = input.LastIndexOf(openBracket);

            if (indexLastOpenBracket == -1)
            {
                return false;
            }

            int indexFirstCloseBracketAfterLastOpenBracket = input.IndexOf(closeBracket, indexLastOpenBracket);

            if (indexFirstCloseBracketAfterLastOpenBracket == -1)
            {
                return true;
            }

            string subexpression = input.Substring(indexLastOpenBracket + 1, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket - 1);

            bool isSubexpression = NotSubexpression(subexpression, allSigns, minus, floatPoint);

            if (isSubexpression)
            {
                return true;
            }

            input = input.Remove(indexLastOpenBracket, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket + 1);
            input = input.Insert(indexFirstCloseBracketAfterLastOpenBracket - (indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket), "0");

            return ExpressionParenthesesNotCorrect(input, openBracket, closeBracket, allSigns, minus, floatPoint);

        }

        private bool NotSubexpression(string subexpression, string allSigns, char minus, char floatPoint)
        {
            SignsMathExpression signsMath = new SignsMathExpression();

            int startIndex = 0;
            int countNumbers = 0;
            int countToken = 0;

            if (string.IsNullOrEmpty(subexpression))
            {
                return true;
            }

            for (int i = 0; i < subexpression.Length; i++)
            {
                if (allSigns.Contains(subexpression[i]) && subexpression[i] != floatPoint)
                {
                    countToken++;
                }

                if (char.IsDigit(subexpression[i]))
                {
                    signsMath.GetStringNumber(subexpression, ref i);

                    countNumbers++;
                }
            }

            if (subexpression[startIndex] == minus)
            {
                countToken--;
            }

            return countNumbers == countToken + 1 ? false : true;
        }
    }
}

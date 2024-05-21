using Calculator.Models;
using System.ComponentModel.DataAnnotations;

namespace Calculator.Validation
{
    public class ValidationMathExpression : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? inputExpression = value as string;

            if (IsInputValid(inputExpression) == false)
            {
                return new ValidationResult("Вы ввели неверное выражение.");
            }

            string replacedInputExpression = inputExpression.Replace(" ", "");

            if (IsFloatingNumber(replacedInputExpression, SignsMathExpression.FloatPoint) == false)
            {
                return new ValidationResult("Вы ввели неверное число с десятичной дробью или поставили знак \",\", вместо \".\".");
            }
            
            if (IsCharactersValid(replacedInputExpression, SignsMathExpression.AllSigns) == false)
            {
                return new ValidationResult("Выражение содержит недопустимые символы.");
            }

            if (IsBracketsBalanceValid(replacedInputExpression, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket) == false)
            {
                return new ValidationResult("Неправильно расставлены скобки.");
            }

            if (IsNoDivisionZero(replacedInputExpression, SignsMathExpression.Divide) == false)
            {
                return new ValidationResult("Нельзя делить на ноль.");
            }

            if (IsSignPlacedCorrectly(replacedInputExpression, SignsMathExpression.AllSigns, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket, SignsMathExpression.Minus) == false)
            {
                return new ValidationResult("В выражении неверно расставлены знаки или числа.");
            }

            if (replacedInputExpression.Any(c => SignsMathExpression.Brackets.Contains(c)) == true)
            {
                if (IsExpressionParenthesesCorrect(replacedInputExpression, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket, SignsMathExpression.AllSigns, SignsMathExpression.Minus, SignsMathExpression.FloatPoint) == false)
                {
                    return new ValidationResult("Выражение в скобках некорректно.");
                }
            }

            return ValidationResult.Success;
        }

        private bool IsInputValid(string? input)
        {
            if (string.IsNullOrWhiteSpace(input) == true)
            {
                return false;
            }

            return true;
        }

        private bool IsFloatingNumber(string input, char floatPoint)
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
                        return false;
                    }
                }
                else
                {
                    isNumber = false;
                    floatPointCount = 0;
                }
            }

            return true;
        }

        private bool IsCharactersValid(string input, string allSymbols)
        {
            if (input.All(c => char.IsDigit(c) || allSymbols.Contains(c)) == false)
            {
                return false;
            }

            return true;
        }

        private bool IsBracketsBalanceValid(string input, char openBracket, char closeBracket)
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
                    return false;
                }
            }

            return balanceOfParentheses == 0;
        }

        private bool IsNoDivisionZero(string input, char signDivide)
        {
            int nextElement = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (i + nextElement < input.Length)
                {
                    if (input[i] == signDivide && input[i + nextElement] == '0')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsSignPlacedCorrectly(string input, string allSigns, char openBracket, char closeBracket, char minus)
        {
            int startIndex = 0;
            int nextIndex = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (allSigns.Contains(input[startIndex]) == true)
                {
                    if (input[startIndex] != minus && input[startIndex] != openBracket)
                    {
                        return false;
                    }
                }
                
                if (i + nextIndex == input.Length)
                {
                    if (allSigns.Contains(input[i]) == true && input[i] != closeBracket)
                    {
                        return false;
                    }
                }

                if (input[i] == openBracket && allSigns.Contains(input[i + nextIndex]) == true)
                {
                    if (input[i + nextIndex] != minus && input[i + nextIndex] != openBracket)
                    {
                        return false;
                    }
                }
                
                if (i + nextIndex < input.Length)
                {
                    if (input[i] == closeBracket && allSigns.Contains(input[i + nextIndex]) == false)
                    {
                        return false;
                    }
                }
                
                if (i + nextIndex < input.Length)
                {
                    if (SignsMathExpression.AllOperations.Contains(input[i]) == true && SignsMathExpression.AllOperations.Contains(input[i + nextIndex]) == true)
                    {
                        if (input[i + nextIndex] != minus || input[i + nextIndex] != closeBracket)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsExpressionParenthesesCorrect(string input, char openBracket, char closeBracket, string allSigns, char minus, char floatPoint)
        {
            int indexLastOpenBracket = input.LastIndexOf(openBracket);

            if (indexLastOpenBracket == -1)
            {
                return true;
            }

            int indexFirstCloseBracketAfterLastOpenBracket = input.IndexOf(closeBracket, indexLastOpenBracket);

            if (indexFirstCloseBracketAfterLastOpenBracket == -1)
            {
                return false;
            }

            string subexpression = input.Substring(indexLastOpenBracket + 1, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket - 1);

            bool isSubexpression = IsSubexpression(subexpression, allSigns, minus, floatPoint);

            if (isSubexpression == false)
            {
                return false;
            }

            input = input.Remove(indexLastOpenBracket, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket + 1);

            return IsExpressionParenthesesCorrect(input, openBracket, closeBracket, allSigns, minus, floatPoint);

        }

        private bool IsSubexpression(string subexpression, string allSigns, char minus, char floatPoint)
        {
            int startIndex = 0;
            int countNumbers = 0;
            int countToken = 0;

            if (subexpression == "")
            {
                return false;
            }

            for (int i = 0; i < subexpression.Length; i++)
            {
                if (char.IsDigit(subexpression[i]))
                {
                    if (IsFloatingNumber(subexpression, floatPoint) == false)
                    {
                        return false;
                    }
                }

                if (allSigns.Contains(subexpression[i]) == true)
                {
                    countToken++;
                }

                if (char.IsDigit(subexpression[i]) == true)
                {
                    countNumbers++;
                }
            }

            if (subexpression[startIndex] == minus)
            {
                countToken--;
            }

            if (countNumbers == countToken + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

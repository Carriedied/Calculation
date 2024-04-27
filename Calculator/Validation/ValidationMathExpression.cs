using Calculator.Models;
using System.ComponentModel.DataAnnotations;

namespace Calculator.Validation
{
    public class ValidationMathExpression : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            SignsMathExpression characters = new SignsMathExpression();

            string? inputExpression = value as string;

            string replacedInputExpression = inputExpression.Replace(" ", "");

            if (IsInputValid(replacedInputExpression) == false)
            {
                return new ValidationResult("Вы ввели неверное выражение.");
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

            if (SignsMathExpression.Brackets.Contains(replacedInputExpression) == true)
            {
                if (IsExpressionParenthesesCorrect(replacedInputExpression, SignsMathExpression.OpenBracket, SignsMathExpression.CloseBracket, SignsMathExpression.AllSigns, SignsMathExpression.Minus) == false)
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
                if (input[i] == signDivide && input[i + nextElement] == '0')
                {
                    return false;
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
                    if (input[startIndex] == minus)
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                
                if (input[i] == openBracket && allSigns.Contains(input[i + nextIndex]) == true)
                {
                    if (input[i + nextIndex] == minus)
                    {
                        continue;
                    }
                    else
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
                        if (input[i + nextIndex] == minus)
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool IsExpressionParenthesesCorrect(string input, char openBracket, char closeBracket, string allSigns, char minus)
        {
            int indexLastOpenBracket = input.LastIndexOf(openBracket);
            int indexFirstCloseBracketAfterLastOpenBracket = 0;
            string subexpression;
            int nextIndex = 1;
            int previousIndex = -1;           

            for (int i = indexLastOpenBracket; i < input.Length; i++)
            {
                if (input[i] == closeBracket)
                {
                    indexFirstCloseBracketAfterLastOpenBracket = i;

                    break;
                }
            }

            subexpression = input.Substring(indexLastOpenBracket + nextIndex, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket + previousIndex);

            bool isSubexpression = IsSubexpression(subexpression, allSigns, minus);

            input = input.Remove(indexLastOpenBracket, indexFirstCloseBracketAfterLastOpenBracket - indexLastOpenBracket - previousIndex);

            if (isSubexpression == true && input.Contains(openBracket) == false)
            {
                return true;
            }
            else if (isSubexpression == true && input.Contains(openBracket) == true)
            {
                IsExpressionParenthesesCorrect(input, openBracket, closeBracket, allSigns, minus);
            }
            else if (isSubexpression == false)
            {
                return false;
            }

            return true;
        }

        private bool IsSubexpression(string subexpression, string allSigns, char minus)
        {
            bool isEnumerationDigitsNumber = false;
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
                    isEnumerationDigitsNumber = true;
                }

                if (allSigns.Contains(subexpression[i]) == true && isEnumerationDigitsNumber == true)
                {
                    countNumbers++;
                    countToken++;
                    isEnumerationDigitsNumber = false;
                }
                else if (allSigns.Contains(subexpression[i]) == true && isEnumerationDigitsNumber == false)
                {
                    if (subexpression[i] == minus && i == startIndex)
                    {
                        continue;
                    }
                    else
                    {
                        countToken++;
                    }
                }
            }

            countNumbers++;

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

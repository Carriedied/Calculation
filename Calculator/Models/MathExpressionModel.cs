using Calculator.Validation;
using System.Linq.Expressions;

namespace Calculator.Models
{
    public class MathExpressionModel
    {
        [ValidationMathExpression(ErrorMessage = "Вы ввели некорректное математическое выражение.")]
        public string? Expression { get; set; }
    }
}

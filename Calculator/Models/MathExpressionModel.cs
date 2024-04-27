using Calculator.Validation;

namespace Calculator.Models
{
    public class MathExpressionModel
    {
        [ValidationMathExpression(ErrorMessage = "Вы ввели некорректное математическое выражение.")]
        public string? Expression { get; set; }
    }
}

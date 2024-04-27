namespace Calculator.Models
{
    public class SignsMathExpression
    {
        public const char OpenBracket = '(';
        public const char CloseBracket = ')';
        public const char Plus = '+';
        public const char Minus = '-';
        public const char Multiply = '*';
        public const char Divide = '/';
        public const char UnaryMinus = '~';
        public const string AllSigns = "()+-*/~";
        public const string AllOperations = "+-*/";
        public const string Brackets = "()";

        private Dictionary<char, int> _operationPriority;

        public SignsMathExpression()
        {
            _operationPriority = new Dictionary<char, int>()
            {
                { OpenBracket, 0},
                { Plus, 1},
                { Minus, 1},
                { Multiply, 2},
                { Divide, 2},
                { UnaryMinus, 3},
            };
        }

        public bool IsThereSignInListOperators(char sign)
        {
            return _operationPriority.ContainsKey(sign);
        }

        public bool IsSavedCharacterPriorityAnotherCharacter(char savedCharacter, char anotherSymbol)
        {
            return _operationPriority[savedCharacter] >= _operationPriority[anotherSymbol];
        }

        public string GetStringNumber(string expression, ref int position)
        {
            string number = "";
            char symbolLine;

            for (; position < expression.Length; position++)
            {
                symbolLine = expression[position];

                if (char.IsDigit(symbolLine))
                {
                    number += symbolLine;
                }
                else
                {
                    position--;

                    break;
                }
            }

            return number;
        }
    }
}

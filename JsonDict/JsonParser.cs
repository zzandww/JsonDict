namespace JsonDict
{
    class JsonParser
    {
        private readonly string json;
        private int index;

        public JsonParser(string jsonString)
        {
            json = jsonString ?? throw new ArgumentNullException(nameof(jsonString));
            index = 0;
        }

        public object Parse()
        {
            SkipWhitespace();

            if (Peek() == '{')
                return ParseObject();
            else if (Peek() == '[')
                return ParseArray();
            else if (Peek() == '"')
                return ParseString();
            else if (Char.IsDigit(Peek()) || Peek() == '-')
                return ParseNumber();
            else if (PeekKeyword("true"))
                return true;
            else if (PeekKeyword("false"))
                return false;
            else if (PeekKeyword("null"))
            {
                ConsumeKeyword("null");
                return null;
            }
            else
                throw new InvalidOperationException($"Unexpected character: {Peek()}");
        }

        

        private Dictionary<string, object> ParseObject()
        {
            Consume('{');
            SkipWhitespace();

            var result = new Dictionary<string, object>();

            while (Peek() != '}')
            {
                string key = ParseString();
                SkipWhitespace();
                Consume(':');
                SkipWhitespace();
                object value = Parse();
                result[key] = value;
                SkipWhitespace();

                if (Peek() == ',')
                {
                    Consume(',');
                    SkipWhitespace();
                }
            }

            Consume('}');
            return result;
        }

        private List<object> ParseArray()
        {
            Consume('[');
            SkipWhitespace();

            var result = new List<object>();

            while (Peek() != ']')
            {
                object value = Parse();
                result.Add(value);
                SkipWhitespace();

                if (Peek() == ',')
                {
                    Consume(',');
                    SkipWhitespace();
                }
            }

            Consume(']');
            return result;
        }

        private string ParseString()
        {
            Consume('"');
            string result = "";

            while (Peek() != '"')
            {
                char currentChar = Consume();

                if (currentChar == '\\') // 处理转义字符
                {
                    char escapedChar = Consume();

                    switch (escapedChar)
                    {
                        case '"':
                            result += '"';
                            break;
                        case '\\':
                            result += '\\';
                            break;
                        case '/':
                            result += '/';
                            break;
                        case 'b':
                            result += '\b';
                            break;
                        case 'f':
                            result += '\f';
                            break;
                        case 'n':
                            result += '\n';
                            break;
                        case 'r':
                            result += '\r';
                            break;
                        case 't':
                            result += '\t';
                            break;
                        case 'u':
                            // 处理 Unicode 字符
                            string unicode = new string(new[] { Consume(), Consume(), Consume(), Consume() });
                            if (int.TryParse(unicode, System.Globalization.NumberStyles.HexNumber, null, out int unicodeValue))
                            {
                                result += (char)unicodeValue;
                            }
                            else
                            {
                                throw new InvalidOperationException($"Invalid Unicode escape sequence: \\u{unicode}");
                            }
                            break;
                        default:
                            // 未知的转义字符
                            throw new InvalidOperationException($"Invalid escape character: \\{escapedChar}");
                    }
                }
                else if (Char.IsControl(currentChar))
                {
                    // JSON 字符串中的控制字符
                    throw new InvalidOperationException($"Invalid control character in string: {currentChar}");
                }
                else
                {
                    result += currentChar;
                }
            }

            Consume('"');
            return result;
        }
        private object ParseNumber()
        {
            string numStr = "";

            if (Peek() == '-')
            {
                numStr += Consume();
            }

            while (Char.IsDigit(Peek()))
            {
                numStr += Consume();
            }

            if (Peek() == '.')
            {
                numStr += Consume();
                while (Char.IsDigit(Peek()))
                {
                    numStr += Consume();
                }
            }

            if (Peek() == 'e' || Peek() == 'E')
            {
                numStr += Consume();
                if (Peek() == '+' || Peek() == '-')
                {
                    numStr += Consume();
                }

                while (Char.IsDigit(Peek()))
                {
                    numStr += Consume();
                }
            }

            if (double.TryParse(numStr, out double result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"Invalid number format: {numStr}");
            }
        }

        private void SkipWhitespace()
        {
            while (index < json.Length && Char.IsWhiteSpace(json[index]))
            {
                index++;
            }
        }

        private char Peek()
        {
            if (index < json.Length)
                return json[index];
            else
                throw new InvalidOperationException("Unexpected end of input.");
        }

        private char Consume()
        {
            char currentChar = Peek();
            index++;
            return currentChar;
        }

        private void Consume(char expectedChar)
        {
            if (Peek() == expectedChar)
                index++;
            else
                throw new InvalidOperationException($"Expected '{expectedChar}', found '{Peek()}'");
        }

        private bool PeekKeyword(string keyword)
        {
            for (int i = 0; i < keyword.Length; i++)
            {
                if (index + i >= json.Length || json[index + i] != keyword[i])
                    return false;
            }
            return true;
        }

        private void ConsumeKeyword(string keyword)
        {
            for (int i = 0; i < keyword.Length; i++)
            {
                Consume(keyword[i]);
            }
        }
    }

}
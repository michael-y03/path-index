
namespace PathIndex.Application.Parsing
{
    internal static class Tokenizer
    {
        public static string[]? TokenizeInput(string input)
        {
            List<string> tokens = [];
            bool insideQuotedArgument = false;
            bool tokenInProgress = false;
            string argument = "";
            foreach (char character in input)
            {

                if (character == '\"')
                {
                    insideQuotedArgument = !insideQuotedArgument;
                    tokenInProgress = true;
                }
                if (insideQuotedArgument && character != '\"')
                    argument += character;
                else if (tokenInProgress && char.IsWhiteSpace(character) && !insideQuotedArgument)
                {
                    tokens.Add(argument);
                    argument = "";
                    tokenInProgress = false;
                }
                else if (character != '\"' && !char.IsWhiteSpace(character))
                {
                    argument += character;
                    tokenInProgress = true;
                }
            }
            if (!insideQuotedArgument && tokenInProgress)
                tokens.Add(argument);
            else
            {
                return null;
            }

            string[] allTokens = [.. tokens];
            return allTokens;
        }
    }
}

using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace Personal.Webspace.Shared.Extensions
{

    public class MarkdigFunctions : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<FunctionParser>())
            {
                pipeline.InlineParsers.InsertBefore<EmphasisInlineParser>(new FunctionParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // Pas besoin d'effectuer d'action particulière pour le rendu
        }
    }


    public class FunctionParser : InlineParser
    {
        public FunctionParser()
        {
            OpeningCharacters = new[] { ':' };
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            var current = slice.CurrentChar;
            if (current == ':' && slice.PeekCharExtra(-1) == ' ')
            {
                var startPosition = slice.Start;
                slice.NextChar();

                var functionNameStart = slice.Start;
                while (!slice.IsEmpty && char.IsLetter(slice.CurrentChar))
                {
                    slice.NextChar();
                }
                var functionName = slice.Text.Substring(functionNameStart, slice.Start - functionNameStart);
                string parameter = string.Empty;

                if (!slice.IsEmpty && slice.CurrentChar == '(')
                {
                    slice.NextChar();

                    if (!slice.IsEmpty && slice.CurrentChar == '\"')
                    {
                        var parameterStart = slice.Start + 1;
                        slice.NextChar();

                        while (!slice.IsEmpty && slice.CurrentChar != '\"')
                        {
                            slice.NextChar();
                        }

                        if (!slice.IsEmpty && slice.CurrentChar == '\"')
                        {
                            parameter = slice.Text.Substring(parameterStart, slice.Start - parameterStart);
                            slice.NextChar();
                        }
                    }

                    if (!slice.IsEmpty && slice.CurrentChar == ')')
                    {
                        var result = ExecuteFunction(functionName, parameter);

                        var htmlInline = new HtmlInline(result)
                        {
                            Line = processor.LineIndex,
                            Column = startPosition
                        };

                        processor.Inline = htmlInline;
                        slice.NextChar();
                        return true;
                    }
                }
            }

            return false;
        }

        private string ExecuteFunction(string functionName, string parameter)
        {
            switch (functionName)
            {
                case "Age":
                    return CalculateAge(parameter);
                case "Image":
                    return DisplayImage(parameter);
                default:
                    return $"Fonction {functionName} inconnue";
            }
        }

        private static string CalculateAge(string dateOfBirth)
        {
            if (DateTime.TryParse(dateOfBirth, out var birthdate))
            {
                var age = DateTime.Now.Year - birthdate.Year;

                if (birthdate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }

                return $"{age} ans";
            }

            return "Date de naissance invalide";
        }

        private static string DisplayImage(string url)
        {
            return $"<img src=\"{url}\" alt=\"Image\" />";
        }
    }

}

using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace Personal.Webspace.Shared.Extensions
{
    public static class MarkdigBadgeConstants
    {
        public const char DelimiterChar = '^';
        public const string ClassToUse = "badge bg-primary";
    }
    public class MarkdigBadge : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<BadgeTextParser>())
            {
                pipeline.InlineParsers.InsertBefore<EmphasisInlineParser>(new BadgeTextParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }
    }

    public class BadgeTextParser : InlineParser
    {
        public BadgeTextParser()
        {
            OpeningCharacters = new[] { MarkdigBadgeConstants.DelimiterChar };
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            var current = slice.CurrentChar;
            if (current == MarkdigBadgeConstants.DelimiterChar)
            {
                slice.NextChar();
                var startPosition = slice.Start;

                while (!slice.IsEmpty && slice.CurrentChar != MarkdigBadgeConstants.DelimiterChar)
                {
                    slice.NextChar();
                }

                if (!slice.IsEmpty && slice.CurrentChar == MarkdigBadgeConstants.DelimiterChar)
                {
                    var text = slice.Text.Substring(startPosition, slice.Start - startPosition);
                    var htmlInline = new HtmlInline($"<span class=\"{MarkdigBadgeConstants.ClassToUse}\">{text}</span>")
                    {
                        Line = processor.LineIndex,
                        Column = startPosition
                    };

                    processor.Inline = htmlInline;
                    slice.NextChar();
                    return true;
                }
            }

            return false;
        }
    }

}

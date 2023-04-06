using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Personal.Webspace.Shared.Extensions
{
    public class MarkdigLinkTargetBlank : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {

        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer? htmlRenderer = renderer as HtmlRenderer;
            if (htmlRenderer != null)
            {
                var inlineRenderer = htmlRenderer.ObjectRenderers.FindExact<LinkInlineRenderer>();

                if (inlineRenderer != null)
                {
                    inlineRenderer.TryWriters.Remove(TryLinkInlineRenderer);
                    inlineRenderer.TryWriters.Add(TryLinkInlineRenderer);
                }
            }
        }

        private bool TryLinkInlineRenderer(HtmlRenderer renderer, LinkInline linkInline)
        {
            if (linkInline.Url == null)
                return false;

            if (!Uri.TryCreate(linkInline.Url, UriKind.RelativeOrAbsolute, out var uri) || !uri.IsAbsoluteUri)
                return false;

            linkInline.SetAttributes(new HtmlAttributes()
            {
                Properties = new List<KeyValuePair<string, string?>>()
                {
                        new ("target", "_blank"),
                        new ("rel", "noopener"),
                }
            });

            return false;
        }
    }
}

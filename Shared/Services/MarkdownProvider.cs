using Markdig;
using Personal.Webspace.Shared.Extensions;

namespace Personal.Webspace.Shared.Services
{
    public class MarkdownProvider
    {
        public MarkdownPipeline Pipeline { get; }

        public MarkdownProvider()
        {
            Pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
                .Use<MarkdigLinkTargetBlank>()
                .Use<MarkdigBadge>()
                .Use<MarkdigFunctions>()
                .Build();
        }
    }
}

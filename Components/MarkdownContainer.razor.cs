using Markdig;
using Microsoft.AspNetCore.Components;
using Personal.Webspace.Shared.Extensions;
using Personal.Webspace.Shared.Services;

namespace Personal.Webspace.Components
{
    public partial class MarkdownContainer
    {
        [Inject]
        private HttpClient Http { get; set; } = null!;

        [Inject]
        private MarkdownProvider MarkdownProvider { get; set; } = null!;

        [EditorRequired]
        [Parameter]
        public string? MarkdownFileName { get; set; }

        private string? HtmlContent { get; set; }

        private MarkupString MarkupContent => new(string.IsNullOrEmpty(HtmlContent)
                                                ? string.Empty
                                                : HtmlContent);

        private bool HasContent => !string.IsNullOrEmpty(HtmlContent);

        private string? _previousMarkdownFileName;

        private bool MarkdownFileNameIsNew()
        {
            if (string.IsNullOrEmpty(MarkdownFileName))
                return false;

            if (string.IsNullOrEmpty(_previousMarkdownFileName) ||
                _previousMarkdownFileName != MarkdownFileName)
            {
                _previousMarkdownFileName = MarkdownFileName;
                return true;
            }

            return false;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (MarkdownFileNameIsNew())
            {
                var markdownContent = await Http.GetStringAsync($"static/{MarkdownFileName}.md");
                HtmlContent = Markdown.ToHtml(markdownContent, MarkdownProvider.Pipeline);
            }
        }

    }
}
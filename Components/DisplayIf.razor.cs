using Microsoft.AspNetCore.Components;

namespace Personal.Webspace.Components
{
    public partial class DisplayIf
    {
        [Parameter]
        public bool Condition { get; set; }

        [EditorRequired]
        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;
        [Parameter]
        public RenderFragment? AlternateContent { get; set; }

        private bool HasAlternateContent => AlternateContent is not null;
    }
}
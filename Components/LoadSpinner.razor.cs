using Microsoft.AspNetCore.Components;

namespace Personal.Webspace.Components
{
    public partial class LoadSpinner
    {
        [Parameter]
        public string? Message { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (string.IsNullOrEmpty(Message))
                Message = "Chargement en cours...";
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestDynamicOdata.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly Microsoft.Extensions.Logging.ILogger<PrivacyModel> _logger;

        public PrivacyModel(Microsoft.Extensions.Logging.ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }

}

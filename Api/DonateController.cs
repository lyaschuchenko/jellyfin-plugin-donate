using Microsoft.AspNetCore.Mvc;
using Jellyfin.Plugin.Donate.Configuration;

namespace Jellyfin.Plugin.Donate.Api
{
    [ApiController]
    [Route("DonatePlugin")]
    public class DonateController : ControllerBase
    {
        [HttpGet("InjectUI.js")]
        [Produces("application/javascript")]
        public IActionResult GetDonateScript()
        {
            var config = Plugin.Instance.Configuration;
            
            string donateUrl = config.DonateUrl?.Trim() ?? "";
            
            if (string.IsNullOrEmpty(donateUrl)) 
            {
                return Content("console.log('Donate Plugin: URL is not configured.');", "application/javascript");
            }

            if (!donateUrl.StartsWith("http", System.StringComparison.OrdinalIgnoreCase))
            {
                donateUrl = "https://" + donateUrl;
            }

            string scriptContent = $$"""
                document.addEventListener('viewshow', function(e) {
                    const isHomePage = e.target.classList.contains('homePage');
                    if (!isHomePage) return;

                    const header = document.querySelector('.headerRight');
                    if (header && !document.getElementById('donate-plugin-icon')) {
                        const btn = document.createElement('button');
                        btn.id = 'donate-plugin-icon';
                        btn.className = 'paper-icon-button-light headerButton';
                        btn.title = 'Support Server';
                        
                        // Іконка сердечка, яка ідеально підходить для Buy Me A Coffee чи банок
                        btn.innerHTML = '<span class="material-icons favorite" style="color: #ff5252;"></span>';
                        
                        btn.onclick = () => window.open('{{donateUrl}}', '_blank');
                        
                        header.prepend(btn);
                    }
                });
            """;

            return Content(scriptContent, "application/javascript");
        }
    }
}
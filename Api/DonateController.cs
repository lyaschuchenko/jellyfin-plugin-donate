using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Обов'язково для AllowAnonymous
using Jellyfin.Plugin.Donate.Configuration;

namespace Jellyfin.Plugin.Donate.Api
{
    [ApiController]
    [Route("DonatePlugin")]
    [AllowAnonymous]
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
                    if (e.target.classList.contains('loginPage')) return;

                    const header = document.querySelector('.headerRight');
                    if (header && !document.getElementById('donate-btn')) {
                        const btn = document.createElement('button');
                        btn.id = 'donate-btn';
                        
                        btn.style.cssText = 'margin: 0 10px; padding: 0 16px; height: 32px; background: rgba(255, 82, 82, 0.15); color: #ff5252; border: 1px solid rgba(255, 82, 82, 0.3); border-radius: 16px; font-weight: 600; cursor: pointer; transition: all 0.2s ease; display: inline-flex; align-items: center; justify-content: center; font-size: 14px; text-transform: uppercase; letter-spacing: 0.5px;';
                        
                        const userLang = (navigator.language || navigator.userLanguage).toLowerCase();
                        const isUA = userLang.includes('uk') || userLang.includes('ru');
                        
                        const hoverText = isUA ? 'Підтримка' : 'Support';
                        const tooltipText = isUA ? 'Підтримка серверу' : 'Support Server';
                        
                        btn.title = tooltipText;
                        btn.innerText = 'Donate';
                        
                        btn.onmouseover = () => {
                            btn.style.background = 'rgba(255, 82, 82, 0.3)';
                            btn.style.borderColor = '#ff5252';
                            btn.innerText = hoverText;
                        };
                        btn.onmouseout = () => {
                            btn.style.background = 'rgba(255, 82, 82, 0.15)';
                            btn.style.borderColor = 'rgba(255, 82, 82, 0.3)';
                            btn.innerText = 'Donate';
                        };
                        
                        btn.onclick = () => window.open('{{donateUrl}}', '_blank');
                        
                        header.prepend(btn);
                    }
                });
            """;

            return Content(scriptContent, "application/javascript");
        }
    }
}
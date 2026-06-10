using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Jellyfin.Plugin.Donate.Configuration;
// Обов'язкові простори імен для контролера:
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Jellyfin.Plugin.Donate
{
    // 1. БЛОК РЕЄСТРАЦІЇ ПЛАГІНА
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public override string Name => "Donate";
        public override Guid Id => Guid.Parse("b7162d9a-f7a9-453f-a621-40c628213b7c");

        public static Plugin Instance { get; private set; }

        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = this.Name,
                    EmbeddedResourcePath = $"{typeof(Plugin).Namespace}.Configuration.configPage.html"
                }
            };
        }
    }

    // 2. БЛОК КОНТРОЛЕРА (Тепер він живе тут і гарантовано скомпілюється)
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

            if (!donateUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
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
                        
                        btn.title = isUA ? 'Підтримка серверу' : 'Support Server';
                        btn.innerText = 'Donate';
                        
                        btn.onmouseover = () => {
                            btn.style.background = 'rgba(255, 82, 82, 0.3)';
                            btn.style.borderColor = '#ff5252';
                            btn.innerText = isUA ? 'Підтримка' : 'Support';
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
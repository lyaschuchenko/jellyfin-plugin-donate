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
            // Отримуємо конфігурацію плагіна
            var config = Plugin.Instance.Configuration;
            
            // Очищаємо рядок (видаляємо '@', якщо адміністратор його додав)
            string venmoHandle = config.VenmoString?.Replace("@", "").Trim() ?? "";
            
            // Захист: якщо адмін ще не налаштував Venmo, скрипт залишається порожнім
            if (string.IsNullOrEmpty(venmoHandle) || venmoHandle == "string") 
            {
                return Content("console.log('Donate Plugin: Venmo is not configured.');", "application/javascript");
            }

            // Використовуємо C# Raw String Literals ($$""") для безпечного вбудовування JS та HTML
            string scriptContent = $$"""
                document.addEventListener('viewshow', function(e) {
                    const isHomePage = e.target.classList.contains('homePage');
                    if (!isHomePage) return;

                    const header = document.querySelector('.headerRight');
                    if (header && !document.getElementById('donate-plugin-icon')) {
                        const btn = document.createElement('button');
                        btn.id = 'donate-plugin-icon';
                        btn.className = 'paper-icon-button-light headerButton';
                        btn.title = 'Donate via Venmo';
                        
                        // Лапки тут тепер повністю безпечні і не ламають код C#
                        btn.innerHTML = '<span class="material-icons attach_money" style="color: #008CFF;"></span>';
                        
                        // Змінна venmoHandle обгорнута у подвійні фігурні дужки для підстановки
                        btn.onclick = () => window.open('https://venmo.com/{{venmoHandle}}', '_blank');
                        
                        header.prepend(btn);
                    }
                });
            """;

            return Content(scriptContent, "application/javascript");
        }
    }
}
using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using Jellyfin.Plugin.Donate.Configuration;

namespace Jellyfin.Plugin.Donate
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public override string Name => "Donate";

        // GUID вашого плагіна з маніфесту
        public override Guid Id => Guid.Parse("b7162d9a-f7a9-453f-a621-40c628213b7c");

        // Цей конструктор вимагається ядром Jellyfin
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public static Plugin Instance { get; private set; }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = this.Name,
                    // Безпечне отримання простору імен (Вирішує помилку CS0120)
                    EmbeddedResourcePath = $"{typeof(Plugin).Namespace}.Configuration.configPage.html"
                }
            };
        }
    }
}
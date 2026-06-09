using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Donate.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public string DonateUrl { get; set; }

        public PluginConfiguration()
        {
            DonateUrl = "";
        }
    }
}
<h1 align="center">Jellyfin Donate Plugin</h1>

<p align="center">
  A lightweight plugin for Jellyfin (10.9+) that seamlessly integrates a "Donate" button into the Web UI header, allowing your users to easily support your server via Venmo.
</p>

## ✨ Features
* **Native UI Integration:** The donate button blends perfectly with the Jellyfin Web UI using native Material Icons.
* **Non-Intrusive:** It appears seamlessly in the top right header without modifying core Jellyfin web files.
* **Easy Configuration:** Set your Venmo handle directly from the Jellyfin Administrator Dashboard.

## 📥 Installation (Recommended)

You can install this plugin directly from the Jellyfin Web UI using the repository manifest.

1. Go to your Jellyfin Server **Dashboard**.
2. Navigate to **Plugins** -> **Repositories**.
3. Click the **+** (Add) button.
4. Enter a name (e.g., `Donate Plugin Repo`).
5. Paste the following Manifest URL:
   ```text
   https://raw.githubusercontent.com/lyaschuchenko/jellyfin-plugin-donate/master/manifest.json
```
6. Save, go to the **Catalog** tab, find "Donate", and click **Install**.
7. Restart your Jellyfin server.

## ⚙️ Configuration & UI Activation

Because Jellyfin backend plugins cannot directly manipulate the frontend DOM by default, you need to add a single line of code to tell the Web UI to load the plugin's script.

1. Go to **Dashboard** -> **Plugins** -> **Donate** and enter your Venmo handle (e.g., `Your-Venmo-Name`). Click Save.
2. Navigate to **Dashboard** -> **General**.
3. Scroll down to the **Custom Web Code** section.
4. Paste the following line into the text box:
   ```html
   <script src="/DonatePlugin/InjectUI.js"></script>
```
5. Click **Save** and refresh your browser (F5). The donate icon should now appear in the top right corner of the home page!

## 🛠️ Manual Build Process

If you prefer to compile the plugin yourself:

1. Clone or download this repository.
2. Ensure you have the **.NET 8.0 SDK** installed (required for Jellyfin 10.9+).
3. Build the plugin with the following command:

   ```sh
   dotnet publish --configuration Release --output bin
```
4. Create a folder named `Donate` in your Jellyfin plugins directory.
5. Move the `Jellyfin.Plugin.Donate.dll` from the `bin` folder into the newly created folder.
6. Restart your Jellyfin server and follow the **Configuration & UI Activation** steps above.

## 👏 Credits
* Original plugin concept and initial backend implementation by [alteredtech](https://github.com/alteredtech/jellyfin-plugin-donate).
* Updated to .NET 8 and refactored with UI DOM injection support in this fork.

---
*Note: This project is independent and not officially affiliated with the core Jellyfin Project.*
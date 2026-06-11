<h1 align="center">Jellyfin Donate Plugin</h1>

<p align="center">
  A lightweight plugin for Jellyfin (10.11+) that seamlessly integrates a "Donate" button into the Web UI header, allowing your users to easily support your server via your preferred platform (e.g., Monobank, Buy Me a Coffee, Patreon).
</p>

## ✨ Features
* **Native UI Integration:** The donate button blends perfectly with the Jellyfin Web UI (modern pill-style button with hover effects).
* **Smart Localization:** Automatically adapts the button text based on the user's browser language (Ukrainian/English).
* **Non-Intrusive:** It appears seamlessly in the top right header without breaking custom CSS themes.
* **Easy Configuration:** Set your custom donation link directly from the Jellyfin Administrator Dashboard.

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

Because modern Jellyfin versions strictly protect their internal configurations (returning 403 Forbidden for non-admin API requests), you cannot inject the script through the Custom CSS field. You need to add a single line of code directly to the Web UI's `index.html` file.

1. Go to **Dashboard** -> **Plugins** -> **Donate** and enter your full donation URL (e.g., [https://send.monobank.ua/](https://send.monobank.ua/)... or your BuyMeACoffee link). Click Save.
2. Locate your Jellyfin `index.html` file. Common paths are:
   * **Linux (Native / APT):** `/usr/share/jellyfin/web/index.html`
   * **Docker:** `/jellyfin/jellyfin-web/index.html`
3. Open `index.html` in a text editor and scroll to the very bottom.
4. Paste the following line directly **before** the closing `</body>` tag:
   ```html
   <script src="/DonatePlugin/InjectUI.js"></script>
   ```
5. Save the file and refresh your Jellyfin home page (press **Ctrl + F5** to clear the cache). The smart donate button will now appear in the top right corner!

## 🛠️ Manual Build Process

If you prefer to compile the plugin yourself:

1. Clone or download this repository.
2. Ensure you have the **.NET 9.0 SDK** installed (required for Jellyfin 10.11+).
3. Build the plugin with the following command:

   ```sh
   dotnet publish --configuration Release
   ```
4. Create a folder named `Donate` in your Jellyfin `plugins` directory.
5. Navigate to the created publish directory in your project folder: `bin/Release/net9.0/publish/`.
6. **Copy ALL files** from the `publish` folder (including `Jellyfin.Plugin.Donate.dll`, `.pdb`, and `.deps.json`) into your newly created `Donate` folder on the server.
7. Restart your Jellyfin server and follow the **Configuration & UI Activation** steps above.

## 👏 Credits
* Original plugin concept and initial backend implementation by [alteredtech](https://github.com/alteredtech/jellyfin-plugin-donate).
* Updated to .NET 9 and completely refactored for universal donation platform support, smart frontend localization, and UI DOM injection by [@lyaschuchenko.](https://github.com/lyaschuchenko).

---
*Note: This project is independent and not officially affiliated with the core Jellyfin Project.*

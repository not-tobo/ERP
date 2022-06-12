using MelonLoader;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Reflection;

namespace ERP
{
    public static class ModInfo
    {
        public const string Name = "ERP";
        public const string Author = "Topi#1337";
        public const string Version = "0.1.0";
        public const string DownloadLink = "https://github.com/not-tobo/ERP";
    }

    public class ERP : MelonMod
    {
        public static MelonLogger.Instance Logger = new MelonLogger.Instance("ERP", ConsoleColor.Magenta);
        public static ReMod.Core.UI.QuickMenu.ReMenuButton ERPBUTTON;
        public static string settingsCategory = "ERP";
        public static int ERPCOUNT;

        public override void OnApplicationStart()
        {
            if (MelonHandler.Plugins.Any(p => p.Info.Name == "ReMod.Core.Updater"))
                return;

            Logger.Msg($"Loading ReMod.Core early so other mods don't break me...");
            DownloadFromGitHub("ReMod.Core", out _);

            MelonCoroutines.Start(StartUiManagerInitIEnumerator());
            Logger.Msg(ConsoleColor.Magenta, "by Topi#1337");
        }

        private IEnumerator StartUiManagerInitIEnumerator()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return null;

            MelonCoroutines.Start(ERP_SOON());
        }

        public static IEnumerator ERP_SOON()
        {
            while (GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)") == null)
                yield return null;

            ERPISINTHEGAME();
        }

        public static void ERPISINTHEGAME()
        {
            //Settins Categorie
            MelonPreferences.CreateCategory(settingsCategory, "ERP");

            //ERP Button Settins Create
            MelonPreferences.CreateEntry<int>(settingsCategory, "ERPcount", 0, "ERP Count");

            //ERP Button get settings
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");

            //setup Buttons
            ERPBUTTON = new ReMod.Core.UI.QuickMenu.ReMenuButton("ERP:\n" + ERPCOUNT, "You did ERP? Click This!", YOU_DID_SHAME, ReMod.Core.VRChat.QuickMenuEx.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").transform, null);
        }

        //ERP Buton logic
        public static void YOU_DID_SHAME()
        {
            ERPCOUNT++;
            Logger.Msg(ConsoleColor.Magenta, "SHAME ON YOU!!!! YOU DID " + ERPCOUNT + " TIMES ERP!");
            MelonPreferences.SetEntryValue(settingsCategory, "ERPcount", ERPCOUNT);
            ERPBUTTON.Text = "ERP: " + ERPCOUNT;
            MelonPreferences.Save();
        }

        internal static class GitHubInfo
        {
            public const string Author = "RequiDev";
            public const string Repository = "ReModCE";
            public const string Version = "latest";
        }

        private void DownloadFromGitHub(string fileName, out Assembly loadedAssembly)
        {
            using var sha256 = SHA256.Create();

            // delete files saved in old path
            if (File.Exists($"{fileName}.dll"))
            {
                File.Delete($"{fileName}.dll");
            }

            byte[] bytes = null;
            var path = Path.Combine("UserLibs", $"{fileName}.dll");
            if (File.Exists(path))
            {
                bytes = File.ReadAllBytes(path);
            }

            using var wc = new WebClient
            {
                Headers =
                {
                    ["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:87.0) Gecko/20100101 Firefox/87.0"
                }
            };

            byte[] latestBytes = null;
            try
            {
                latestBytes = wc.DownloadData($"https://github.com/{GitHubInfo.Author}/{GitHubInfo.Repository}/releases/{GitHubInfo.Version}/download/{fileName}.dll");
            }
            catch (WebException e)
            {
                MelonLogger.Error($"Unable to download latest version of ReModCE: {e}");
            }

            if (bytes == null)
            {
                if (latestBytes == null)
                {
                    MelonLogger.Error($"No local file exists and unable to download latest version from GitHub. {fileName} will not load!");
                    loadedAssembly = null;
                    return;
                }
                MelonLogger.Warning($"Couldn't find {fileName}.dll on disk. Saving latest version from GitHub.");
                bytes = latestBytes;
                try
                {
                    File.WriteAllBytes(path, bytes);
                }
                catch (IOException e)
                {
                    Logger.Warning($"Failed writing {fileName} to disk. You may encounter errors while using ReModCE.");
                }
            }

            try
            {
                loadedAssembly = Assembly.Load(bytes);
            }
            catch (BadImageFormatException e)
            {
                MelonLogger.Error($"Couldn't load specified image: {e}");
                loadedAssembly = null;
            }
        }
    }
}

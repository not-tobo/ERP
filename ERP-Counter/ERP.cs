using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubyButtonAPI;
using MelonLoader;
using System.Collections;

namespace ERP_Counter
{
    public static class ModInfo
    {
        public const string Name = "ERP";
        public const string Author = "Topi#1337";
        public const string Version = "0.0.2";
        public const string DownloadLink = "näh";
    }
    public class ERP : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(StartUiManagerInitIEnumerator());
            MelonLogger.Msg(ConsoleColor.Magenta, "by Topi#1337");
        }

        private IEnumerator StartUiManagerInitIEnumerator()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return null;

            ERPISINTHEGAME();
        }

        public static void ERPISINTHEGAME()
        {
            MelonPreferences.CreateCategory(settingsCategory, "ERP");
            MelonPreferences.CreateEntry<int>(settingsCategory, "ERPcount", 0, "ERP Count");
            MelonPreferences.CreateEntry<int>(settingsCategory, "uiMenuX", 0, "UI Button X");
            MelonPreferences.CreateEntry<int>(settingsCategory, "uiMenuY", 1, "UI Button Y");
            MelonPreferences.CreateEntry<bool>(settingsCategory, "halfButton", false, "UI half Button");
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");
            uiMenuX = MelonPreferences.GetEntryValue<int>(settingsCategory, "uiMenuX");
            uiMenuY = MelonPreferences.GetEntryValue<int>(settingsCategory, "uiMenuY");
            halfButton = MelonPreferences.GetEntryValue<bool>(settingsCategory, "halfButton");

            MelonLogger.Msg(ConsoleColor.Magenta, "ERP BUTTON COORDS X: " + uiMenuX + " Y: " + uiMenuY);
            MelonLogger.Msg(ConsoleColor.Magenta, "YOU START WITH A COUNT OF " + ERPCOUNT + " TODAY. WILL IT INCREASE?");

            ERPBUTTON = new QMSingleButton("ShortcutMenu", uiMenuX, uiMenuY, "ERP: " + ERPCOUNT, YOU_DID_SHAME, "You did ERP? Click This!", null, null, halfButton);
        }

        public static void YOU_DID_SHAME()
        {
            ERPCOUNT = ERPCOUNT + 1;
            MelonLogger.Msg(ConsoleColor.Magenta, "SHAME ON YOU!!!! YOU DID " + ERPCOUNT + " TIMES ERP!");
            MelonPreferences.SetEntryValue(settingsCategory, "ERPcount", ERPCOUNT);
            MelonPreferences.Save();
        }

        public override void OnPreferencesSaved()
        {
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");
            ERPBUTTON.setButtonText("ERP:" + ERPCOUNT);
        }

        public static QMSingleButton ERPBUTTON;
        public static int ERPCOUNT;
        public static string settingsCategory = "ERP";
        public static int uiMenuX;
        public static int uiMenuY;
        public static bool halfButton;
    }
}

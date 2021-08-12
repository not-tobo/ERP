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
        public const string Version = "0.0.3";
        public const string DownloadLink = "https://github.com/not-tobo/ERP";
    }
    public class ERP : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonCoroutines.Start(StartUiManagerInitIEnumerator());
            MelonLogger.Msg(ConsoleColor.Magenta, "by Topi#1337");
        }

        //wait for vrc ui
        private IEnumerator StartUiManagerInitIEnumerator()
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return null;

            ERPISINTHEGAME();
        }

        //start the the mop
        public static void ERPISINTHEGAME()
        {
            //Settins Categorie
            MelonPreferences.CreateCategory(settingsCategory, "ERP");
            
            //ERP Button Settins Create
            MelonPreferences.CreateEntry<int>(settingsCategory, "ERPcount", 0, "ERP Count");
            MelonPreferences.CreateEntry<float>(settingsCategory, "uiErpMenuX", 0, "UI ERP Button X");
            MelonPreferences.CreateEntry<float>(settingsCategory, "uiErpMenuY", 0, "UI ERP Button Y");

            //Wholesome Button Settins Create
            MelonPreferences.CreateEntry<int>(settingsCategory, "Wcount", 0, "Wholesome Count");
            MelonPreferences.CreateEntry<float>(settingsCategory, "uiWMenuX", 0, "UI Wholesome Button X");
            MelonPreferences.CreateEntry<float>(settingsCategory, "uiWMenuY", 1.5f, "UI Wholesome Button Y");

            //enable Wholesome Button
            MelonPreferences.CreateEntry<bool>(settingsCategory, "wholesomeMode", false, "Enable Wholesome Button");

            //ERP Button get settings
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");
            uiErpMenuX = MelonPreferences.GetEntryValue<float>(settingsCategory, "uiErpMenuX");
            uiErpMenuY = MelonPreferences.GetEntryValue<float>(settingsCategory, "uiErpMenuY");

            //Wholesome Button get settings
            WCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "Wcount");
            uiWMenuX = MelonPreferences.GetEntryValue<float>(settingsCategory, "uiWMenuX");
            uiWMenuY = MelonPreferences.GetEntryValue<float>(settingsCategory, "uiWMenuY");
            
            //get if wholesome Mode is enabled
            wholesomeMode = MelonPreferences.GetEntryValue<bool>(settingsCategory, "wholesomeMode");

            //start console output Buttons
            MelonLogger.Msg(ConsoleColor.Magenta, "ERP BUTTON COORDS X: " + uiErpMenuX + " Y: " + uiErpMenuY);
            if (wholesomeMode)
                MelonLogger.Msg(ConsoleColor.Magenta, "WHOLESOME BUTTON COORDS X: " + uiWMenuX + " Y: " + uiWMenuY);
            
            //start console output Count
            MelonLogger.Msg(ConsoleColor.Magenta, "YOU START WITH A ERP COUNT OF " + ERPCOUNT + " TODAY. WILL IT INCREASE?");
            if (wholesomeMode)
                MelonLogger.Msg(ConsoleColor.Magenta, "YOU START WITH A WHOLESOME COUNT OF " + WCOUNT + " TODAY. WILL IT INCREASE?");

            //setup Buttons
            ERPBUTTON = new QMSingleButton("ShortcutMenu", uiErpMenuX, uiErpMenuY, "ERP: " + ERPCOUNT, YOU_DID_SHAME, "You did ERP? Click This!", null, null, wholesomeMode);
            if (wholesomeMode)
                WBUTTON = new QMSingleButton("ShortcutMenu", uiWMenuX, uiWMenuY, "Wholesome:\n" + WCOUNT, YOU_UWU, "You did something Wholesome or soemthing Wholesome happend to you? Click This!", null, null, wholesomeMode);
        }

        //ERP Buton logic
        public static void YOU_DID_SHAME()
        {
            ERPCOUNT = ERPCOUNT + 1;
            MelonLogger.Msg(ConsoleColor.Magenta, "SHAME ON YOU!!!! YOU DID " + ERPCOUNT + " TIMES ERP!");
            MelonPreferences.SetEntryValue(settingsCategory, "ERPcount", ERPCOUNT);
            MelonPreferences.Save();
        }

        //Wholesome Button logic
        public static void YOU_UWU()
        {
            WCOUNT = WCOUNT + 1;
            MelonLogger.Msg(ConsoleColor.Magenta, "OH THAT WAS WHOLESOME!!!! WHOLESOME MOMENTS YOU HAD " + ERPCOUNT + "!");
            MelonPreferences.SetEntryValue(settingsCategory, "Wcount", WCOUNT);
            MelonPreferences.Save();
        }

        public override void OnPreferencesSaved()
        {
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");
            ERPBUTTON.setButtonText("ERP: " + ERPCOUNT);

            WCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "Wcount");
            WBUTTON.setButtonText("Wholesome:\n" + WCOUNT);
        }

        public static QMSingleButton ERPBUTTON;
        public static QMSingleButton WBUTTON;

        public static string settingsCategory = "ERP";

        public static int ERPCOUNT;
        public static float uiErpMenuX;
        public static float uiErpMenuY;

        public static int WCOUNT;
        public static float uiWMenuX;
        public static float uiWMenuY;

        public static bool wholesomeMode;
    }
}

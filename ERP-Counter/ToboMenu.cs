using MelonLoader;
using System;
using System.Collections;
using UnityEngine.UI;
using ReButtonAPI;
using UnityEngine;
using VRC.UI.Elements;

namespace ERP
{
    public static class ModInfo
    {
        public const string Name = "ERP";
        public const string Author = "Topi#1337";
        public const string Version = "0.0.4";
        public const string DownloadLink = "https://github.com/not-tobo/ERP";
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

            //Wholesome Button Settins Create
            MelonPreferences.CreateEntry<int>(settingsCategory, "Wcount", 0, "Wholesome Count");

            //enable Wholesome Button
            MelonPreferences.CreateEntry<bool>(settingsCategory, "wholesomeMode", false, "Enable Wholesome Button");

            //ERP Button get settings
            ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");

            //Wholesome Button get settings
            WCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "Wcount");

            //get if wholesome Mode is enabled
            wholesomeMode = MelonPreferences.GetEntryValue<bool>(settingsCategory, "wholesomeMode");

            //setup Buttons
            ERPBUTTON = new ReMenuButton("ERP_btn", "ERP:\n" + ERPCOUNT, "You did ERP? Click This!", YOU_DID_SHAME, ExtendedQuickMenu.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").transform, null);
            if (wholesomeMode)
                WBUTTON = new ReMenuButton("ERP_btn", "ERP: " + "Wholesome:\n" + WCOUNT, "You did something Wholesome or soemthing Wholesome happend to you? Click This!", YOU_UWU, ExtendedQuickMenu.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").transform, null);


            //Thx to Requi :3
            var dashboard = ExtendedQuickMenu.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard").GetComponent<UIPage>();
            var scrollRect = dashboard.GetComponentInChildren<ScrollRect>();
            var dashboardScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();

            var dashboardContent = scrollRect.content;
            dashboardContent.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            dashboardContent.Find("Carousel_Banners").gameObject.SetActive(false);

            scrollRect.enabled = true;
            scrollRect.verticalScrollbar = dashboardScrollbar;
            scrollRect.viewport.GetComponent<RectMask2D>().enabled = true;
        }
        //ERP Buton logic
        public static void YOU_DID_SHAME()
        {
            ERPCOUNT++;
            MelonLogger.Msg(ConsoleColor.Magenta, "SHAME ON YOU!!!! YOU DID " + ERPCOUNT + " TIMES ERP!");
            MelonPreferences.SetEntryValue(settingsCategory, "ERPcount", ERPCOUNT);
            ERPBUTTON.Text = "ERP: " + ERPCOUNT;
            MelonPreferences.Save();
        }
        //Wholesome Button logic
        public static void YOU_UWU()
        {
            WCOUNT++;
            MelonLogger.Msg(ConsoleColor.Magenta, "OH THAT WAS WHOLESOME!!!! WHOLESOME MOMENTS YOU HAD " + WCOUNT + "!");
            MelonPreferences.SetEntryValue(settingsCategory, "Wcount", WCOUNT);
            MelonPreferences.Save();
            WBUTTON.Text = "Wholesome:\n" + WCOUNT;
        }
        public override void OnPreferencesSaved()
        {
            //ERPCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "ERPcount");
            //ERPBUTTON.Text = "ERP: " + ERPCOUNT;

            //WCOUNT = MelonPreferences.GetEntryValue<int>(settingsCategory, "Wcount");
            //WBUTTON.Text = "Wholesome:\n" + WCOUNT;
        }

        public static ReMenuButton ERPBUTTON;
        public static ReMenuButton WBUTTON;

        public static string settingsCategory = "ERP";

        public static int ERPCOUNT;

        public static int WCOUNT;

        public static bool wholesomeMode;
    }
}

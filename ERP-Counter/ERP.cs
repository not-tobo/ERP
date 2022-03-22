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
        public const string Version = "0.0.5";
        public const string DownloadLink = "https://github.com/not-tobo/ERP";
    }

    public class ERP : MelonMod
    {
        public static MelonLogger.Instance Logger = new MelonLogger.Instance("ERP", ConsoleColor.Magenta);
        public static ReMenuButton ERPBUTTON;
        public static string settingsCategory = "ERP";
        public static int ERPCOUNT;

        public override void OnApplicationStart()
        {
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
            ERPBUTTON = new ReMenuButton("ERP_btn", "ERP:\n" + ERPCOUNT, "You did ERP? Click This!", YOU_DID_SHAME, ExtendedQuickMenu.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").transform, null);

            //Thx to Requi :3
            var dashboard = ExtendedQuickMenu.Instance.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard").GetComponent<UIPage>();
            var scrollRect = dashboard.GetComponentInChildren<ScrollRect>();
            var dashboardScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();

            var dashboardContent = scrollRect.content;
            dashboardContent.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
            dashboardContent.Find("Carousel_Banners")?.gameObject.SetActive(false);

            scrollRect.enabled = true;
            scrollRect.verticalScrollbar = dashboardScrollbar;
            scrollRect.viewport.GetComponent<RectMask2D>().enabled = true;
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
    }
}

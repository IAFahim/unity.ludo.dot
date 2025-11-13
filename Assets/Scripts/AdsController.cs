/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ✅ Added to fix ShowResult reference in Unity 6.2
using UnityEngine.Advertisements;
using UnityEngine.UI;
using AssemblyCSharp;

public class AdsController : MonoBehaviour
{

    [Header("AD mediation Type")]
    [Header("SEQUENCE - display networks in sequence")]
    [Header("FALLBACK - Try to load network with order 1, if no fill then next network")]
    [Space(10)]
    public DisplayType displayType = DisplayType.SEQUENCE;

    [Header("Order of networks. Set 0 to disable network")]
    [Space(10)]
    [Range(0, 3)]
    public int AudienceNetworkOrder = 1;
    [Range(0, 3)]
    public int AdmobOrder = 2;
    [Range(0, 3)]
    public int ChartboostOrder = 3;

    private static int activeNetworks = 3;


    [Header("Networks IDs Android")]
    [Space(10)]
    public string FANAndroidID;
    public string AdmobAndroidID;
    public string ChartboostAndroidID;
    public string ChartboostAndroidSignature;

    [Header("Networks IDs iOS")]
    [Space(10)]
    public string FAN_IOS_ID;
    public string AdmobIOSID;
    public string ChartboostIOSID;
    public string ChartboostIOSSignature;

    [Header("Show Ads in locations")]
    [Space(10)]
    public bool ShowAdOnMenuScene = false;
    [HideInInspector]
    public bool ShowAdOnGameOver = false;
    [HideInInspector]
    public bool ShowAdOnPause = false;
    [HideInInspector]
    public bool ShowAdOnLevelFinish = false;
    public bool ShowAdOnFacebookFriends = false;
    public bool ShowAdOnGameFinishWindow = false;
    public bool ShowAdOnStoreWindow = false;
    public bool ShowAdOnGamePropertiesWindow = false;

    [Header("Should Show Ad In Menu Scene after game start?")]
    [Space(10)]
    public bool loadAdInMenuAfterStart = true;

    private int NetworksCount = 3;
    private static GameObject go;

    [HideInInspector]
    public enum DisplayType
    {
        SEQUENCE, FALLBACK
    }
    private int counter = 0;

    private static adNetwork[] networks;

    private adNetwork[] networksInit;

    private static int currentAdIndex = 0;

    private static int displayAttempts = 0;
    private int displayCount = 1;

    private static string FBAudienceID;
    private static string AdMobAndroid_ID;
    private static string AdMobIOS_ID;
    public static string ChartboostAndroid_ID;
    public static string ChartboostAndroid_Signature;
    public static string ChartboostIOS_ID;
    public static string ChartboostIOS_Signature;
    public static string FANAndroid_ID;
    public static string FANIOS_ID;
    private int menuLoadCount = 0;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        GameManager.Instance.adsController = this;

        FANAndroid_ID = FANAndroidID;
        FANIOS_ID = FAN_IOS_ID;
        AdMobAndroid_ID = AdmobAndroidID;
        AdMobIOS_ID = AdmobIOSID;
        ChartboostAndroid_ID = ChartboostAndroidID;
        ChartboostAndroid_Signature = ChartboostAndroidSignature;
        ChartboostIOS_ID = ChartboostIOSID;
        ChartboostIOS_Signature = ChartboostIOSSignature;

        go = gameObject;

        networks = new adNetwork[NetworksCount];
        networksInit = new adNetwork[NetworksCount];

        // networksInit[0] = new FacebookAudienceAdNetwork();
        // networksInit[1] = new AdMobAdNetwork();
        // networksInit[2] = new ChartboostAdNetwork();

        for (int i = 0; i < networks.Length; i++)
        {
            if (networksInit[i] != null)
            {
                try
                {
                    networksInit[i].init();
                }
                catch (Exception e) { }
                networks[i] = networksInit[i];
            }
        }

        parseStringAndSortNetworks(AudienceNetworkOrder + ";" + AdmobOrder + ";" + ChartboostOrder);
    }

#if UNITY_ADS || UNITY_ADS_INCLUDED
    public void ShowVideoAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            // ✅ Explicit ShowOptions using correct namespace
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            Debug.LogWarning("Rewarded video not ready");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Rewarded Ad Finished");
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Rewarded Ad Skipped");
                break;
            case ShowResult.Failed:
                Debug.LogError("Rewarded Ad Failed to show");
                break;
        }
    }
#endif

    public void loadAd(AdLocation location)
    {
        if (location == AdLocation.GameOver && !ShowAdOnGameOver) return;
        if (location == AdLocation.GameStart && !ShowAdOnMenuScene) return;
        if (location == AdLocation.Pause && !ShowAdOnPause) return;
        if (location == AdLocation.LevelComplete && !ShowAdOnLevelFinish) return;
        if (location == AdLocation.FacebookFriends && !ShowAdOnFacebookFriends) return;
        if (location == AdLocation.GameFinishWindow && !ShowAdOnGameFinishWindow) return;
        if (location == AdLocation.StoreWindow && !ShowAdOnStoreWindow) return;
        if (location == AdLocation.GamePropertiesWindow && !ShowAdOnGamePropertiesWindow) return;

        if (location == AdLocation.GameStart)
        {
            menuLoadCount++;
            if (!loadAdInMenuAfterStart && menuLoadCount < 2)
            {
                Debug.Log("Skip AD on game start");
                return;
            }
            Debug.Log("Load AD Game start");
        }

        if (PlayerPrefs.GetInt(StaticStrings.PrefsPlayerRemovedAds) == 0)
        {
            displayAttempts = 0;

            if (displayType == DisplayType.SEQUENCE)
            {
                networks[currentAdIndex].loadAd();
            }
            else if (displayType == DisplayType.FALLBACK)
            {
                currentAdIndex = 0;
                networks[currentAdIndex].loadAd();
            }

            displayCount++;
        }
    }

    private abstract class adNetwork : IAdNetwork
    {
        public abstract void init();

        public abstract void loadAd();

        public abstract void destroyAd();
        public void loadNextNetwork()
        {
            displayAttempts++;
            if (displayAttempts < activeNetworks)
            {
                for (int i = 0; i < networks.Length; i++)
                {
                    currentAdIndex = (currentAdIndex + 1) % activeNetworks;
                    if (networks[currentAdIndex] != null)
                    {
                        networks[currentAdIndex].loadAd();
                        break;
                    }
                }
            }
        }
    }

    private interface IAdNetwork
    {
        void init();
        void loadAd();
        void loadNextNetwork();
    }

    void OnDestroy()
    {
        for (int i = 0; i < networks.Length; i++)
        {
            if (networks[i] != null)
                networks[i].destroyAd();
        }
        Debug.Log("InterstitialAdTest was destroyed!");
    }

    public void parseStringAndSortNetworks(String sequence)
    {
        Debug.Log("Parsing mediation networks");
        try
        {
            String[] input = sequence.ToLower().Split(';');

            if (input.Length == networks.Length)
            {
                int lastIndex = networks.Length - 1;

                for (int i = 0; i < networks.Length; i++)
                {
                    if (int.Parse(input[i]) > 0)
                    {
                        networks[int.Parse(input[i]) - 1] = networksInit[i];
                    }
                    else
                    {
                        networks[lastIndex] = null;
                        activeNetworks--;
                        lastIndex--;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error parsing configuration file:\n" + e.ToString());
        }
    }
}

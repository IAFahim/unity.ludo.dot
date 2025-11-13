/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowController : MonoBehaviour
{

    public GameObject Sounds;
    public GameObject Vibrations;
    public GameObject Notifications;
    public GameObject FriendsRequests;
    public GameObject PrivateRoomRequests;



    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt(StaticStrings.SoundsKey, 0) == 1)
        {
            Sounds.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt(StaticStrings.NotificationsKey, 0) == 1)
        {
            Notifications.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt(StaticStrings.VibrationsKey, 0) == 1)
        {
            Vibrations.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt(StaticStrings.FriendsRequestesKey, 0) == 1)
        {
            FriendsRequests.GetComponent<Toggle>().isOn = false;
        }

        if (PlayerPrefs.GetInt(StaticStrings.PrivateRoomKey, 0) == 1)
        {
            PrivateRoomRequests.GetComponent<Toggle>().isOn = false;
        }

        Sounds.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        Notifications.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        Vibrations.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        FriendsRequests.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        PrivateRoomRequests.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();

        Sounds.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetInt(StaticStrings.SoundsKey, value ? 0 : 1);
                    if (value)
                    {
                        AudioListener.volume = 1;
                    }
                    else
                    {
                        AudioListener.volume = 0;
                    }
                }
        );

        Notifications.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetInt(StaticStrings.NotificationsKey, value ? 0 : 1);
                    if (!value)
                    {
                        Debug.Log("Clear notifications!");
                        //LocalNotification.CancelNotification(1);
                    }
                    else
                    {
                        // GameObject fortune = GameObject.Find("FortuneWheelWindow");
                        // if (fortune != null)
                        // {
                        //     fortune.GetComponent<FortuneWheelManager>().SetNextFreeTime();
                        // }
                    }
                }
        );

        Vibrations.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetInt(StaticStrings.VibrationsKey, value ? 0 : 1);
                }
        );

        FriendsRequests.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetInt(StaticStrings.FriendsRequestesKey, value ? 0 : 1);
                }
        );

        PrivateRoomRequests.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
                {
                    PlayerPrefs.SetInt(StaticStrings.PrivateRoomKey, value ? 0 : 1);
                }
        );

    }


}

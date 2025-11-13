/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class StartScriptController : MonoBehaviour
{

    public GameObject splashCanvas;
    public GameObject LoginCanvas;

    public GameObject menuCanvas;
    public GameObject[] go;

    public GameObject fbButton;

    public GameObject fbLoginCoinText;
    public GameObject guestLoginCoinText;

    // Use this for initialization
    void Start()
    {

        fbLoginCoinText.GetComponent<Text>().text = StaticStrings.initCoinsCountFacebook.ToString();
        guestLoginCoinText.GetComponent<Text>().text = StaticStrings.initCoinsCountGuest.ToString();

        Debug.Log("START SCRIPT");
        if (PlayerPrefs.HasKey("LoggedType"))
        {
            splashCanvas.SetActive(true);
        }
        else
        {
            LoginCanvas.SetActive(true);
        }

#if UNITY_WEBGL
        fbButton.SetActive(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideAllElements()
    {
        menuCanvas.SetActive(true);
    }
}

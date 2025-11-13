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
using System.Collections.Generic;

public class SetMyData : MonoBehaviour
{

    public GameObject avatar;
    public GameObject name;
    public GameObject matchCanvas;
    public GameObject controlAvatars;
    public GameObject backButton;


    // Use this for initialization


    public void MatchPlayer()
    {

        //name.GetComponent<Text>().text = GameManager.Instance.nameMy;
        if (GameManager.Instance.avatarMy != null)
            avatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;


        controlAvatars.GetComponent<ControlAvatars>().reset();

    }

    public void setBackButton(bool active)
    {
        backButton.SetActive(active);
    }
}

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
using UnityEngine;
using UnityEngine.UI;

public class MenuEffect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GetComponent<Image>().enabled = false;
        Invoke("start", Random.Range(0.0f, 1f));
    }

    private void start()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<Animator>().speed = Random.Range(0.1f, 0.3f);
        GetComponent<Animator>().Play("Menu_Effect");
    }

}

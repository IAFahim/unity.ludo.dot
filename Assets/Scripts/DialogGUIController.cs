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

public class DialogGUIController : MonoBehaviour
{
    public static DialogGUIController instance = null;
    public GameObject Other;
    // Use this for initialization
    void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        // if (FindObjectsOfType(GetType()).Length > 1)
        // {
        //     Destroy(gameObject);
        // }
        //Check if instance already exists
        if (instance == null)
        {

            //if not, set instance to this
            instance = this;
           // Other.GetComponent<AdMobObjectController>().Init();
            //If instance already exists and it's not this:
        }
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);


    }

    // Update is called once per frame
    void Update()
    {

    }
}

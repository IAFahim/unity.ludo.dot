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

public class ConnectionLostController : MonoBehaviour {

    // Use this for initialization
    public GameObject canvas;

    void Start() {
        DontDestroyOnLoad(transform.gameObject);
        GameManager.Instance.connectionLost = this;

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            showDialog();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void destroy() {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    public void showDialog() {
        canvas.SetActive(true);
    }

    public void closeApp() {
        Application.Quit();
    }
}

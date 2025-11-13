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

[RequireComponent(typeof(InputToEvent))]
public class PointedAtGameObjectInfo : MonoBehaviour 
{
    void OnGUI()
    {
        if (InputToEvent.goPointedAt != null)
        {
            PhotonView pv = InputToEvent.goPointedAt.GetPhotonView();
            if (pv != null)
            {
                GUI.Label(new Rect(Input.mousePosition.x + 5, Screen.height - Input.mousePosition.y - 15, 300, 30), string.Format("ViewID {0} {1}{2}", pv.viewID, (pv.isSceneView) ? "scene " : "", (pv.isMine) ? "mine" : "owner: " + pv.ownerId));
            }
        }
    }

}

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

[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
    public GameObject Prefab;

    public void AllocateManualPhotonView()
    {
        PhotonView pv = this.gameObject.GetPhotonView();
        if (pv == null)
        {
            Debug.LogError("Can't do manual instantiation without PhotonView component.");
            return;
        }

        int viewID = PhotonNetwork.AllocateViewID();
        pv.RPC("InstantiateRpc", PhotonTargets.AllBuffered, viewID);
    }

    [PunRPC]
    public void InstantiateRpc(int viewID)
    {
        GameObject go = GameObject.Instantiate(Prefab, InputToEvent.inputHitPos + new Vector3(0, 5f, 0), Quaternion.identity) as GameObject;
        go.GetPhotonView().viewID = viewID;

        OnClickDestroy ocd = go.GetComponent<OnClickDestroy>();
        ocd.DestroyByRpc = true;
    }
}

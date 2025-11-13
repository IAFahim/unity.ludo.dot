/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System.Text;
using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;

public class SupportLogger : MonoBehaviour
{
    public bool LogTrafficStats = true;

    public void Start()
    {
        GameObject go = GameObject.Find("PunSupportLogger");
        if (go == null)
        {
            go = new GameObject("PunSupportLogger");
            DontDestroyOnLoad(go);
            SupportLogging sl = go.AddComponent<SupportLogging>();
            sl.LogTrafficStats = this.LogTrafficStats;
        }
    }
}

public class SupportLogging : MonoBehaviour
{
    public bool LogTrafficStats;

    public void Start()
    {
        if (LogTrafficStats)
        {
            this.InvokeRepeating("LogStats", 10, 10);
        }
    }


    protected void OnApplicationPause(bool pause)
    {
        Debug.Log("SupportLogger OnApplicationPause: " + pause + " connected: " + PhotonNetwork.connected);
    }

    public void OnApplicationQuit()
    {
        this.CancelInvoke();
    }

    public void LogStats()
    {
        if (this.LogTrafficStats)
        {
            Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
        }
    }

    private void LogBasics()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SupportLogger Info: PUN {0}: ", PhotonNetwork.versionPUN);

        sb.AppendFormat("AppID: {0}*** GameVersion: {1} PeerId: {2} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion, PhotonNetwork.networkingPeer.PeerID);
        sb.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
        sb.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);


        Debug.Log(sb.ToString());
    }


    public void OnConnectedToPhoton()
    {
        Debug.Log("SupportLogger OnConnectedToPhoton().");
        this.LogBasics();

        if (LogTrafficStats)
        {
            PhotonNetwork.NetworkStatisticsEnabled = true;
        }
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log("SupportLogger OnFailedToConnectToPhoton("+cause+").");
        this.LogBasics();
    }

    public void OnJoinedLobby()
    {
        Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby + ").");
    }

    public void OnJoinedRoom()
    {
        Debug.Log("SupportLogger OnJoinedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
    }

    public void OnCreatedRoom()
    {
        Debug.Log("SupportLogger OnCreatedRoom(" + PhotonNetwork.room + "). " + PhotonNetwork.lobby + " GameServer:" + PhotonNetwork.ServerAddress);
    }

    public void OnLeftRoom()
    {
        Debug.Log("SupportLogger OnLeftRoom().");
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
    }
}

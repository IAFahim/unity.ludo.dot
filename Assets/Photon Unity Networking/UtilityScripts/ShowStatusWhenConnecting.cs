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

public class ShowStatusWhenConnecting : MonoBehaviour 
{
    public GUISkin Skin;

    void OnGUI()
    {
        if( Skin != null )
        {
            GUI.skin = Skin;
        }

        float width = 400;
        float height = 100;

        Rect centeredRect = new Rect( ( Screen.width - width ) / 2, ( Screen.height - height ) / 2, width, height );

        GUILayout.BeginArea( centeredRect, GUI.skin.box );
        {
            GUILayout.Label( "Connecting" + GetConnectingDots(), GUI.skin.customStyles[ 0 ] );
            GUILayout.Label( "Status: " + PhotonNetwork.connectionStateDetailed );
        }
        GUILayout.EndArea();

        if( PhotonNetwork.inRoom )
        {
            enabled = false;
        }
    }

    string GetConnectingDots()
    {
        string str = "";
        int numberOfDots = Mathf.FloorToInt( Time.timeSinceLevelLoad * 3f % 4 );

        for( int i = 0; i < numberOfDots; ++i )
        {
            str += " .";
        }

        return str;
    }
}

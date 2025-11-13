/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEditor;
using UnityEngine;

namespace PlayFab.Editor
{
    public class PlayFabHelp : EditorWindow
    {
        [MenuItem("PlayFab/GettingStarted")]
        private static void GettingStarted()
        {
            Application.OpenURL("https://playfab.com/docs/getting-started-with-playfab/");
        }

        [MenuItem("PlayFab/Docs")]
        private static void Documentation()
        {
            Application.OpenURL("https://api.playfab.com/documentation");
        }

        [MenuItem("PlayFab/Dashboard")]
        private static void Dashboard()
        {
            Application.OpenURL("https://developer.playfab.com/");
        }
    }
}

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using PlayFab;

namespace PlayFab.Internal
{
    public class Log
    {
        public static void Debug(string text, params object[] args)
        {
            if ((PlayFabSettings.LogLevel & PlayFabLogLevel.Debug) != 0)
            {
                UnityEngine.Debug.Log(Util.timeStamp + " DEBUG: " + Util.Format(text, args));
            }
        }

        public static void Info(string text, params object[] args)
        {
            if ((PlayFabSettings.LogLevel & PlayFabLogLevel.Info) != 0)
            {
                UnityEngine.Debug.Log(Util.timeStamp + " INFO: " + Util.Format(text, args));
            }
        }

        public static void Warning(string text, params object[] args)
        {
            if ((PlayFabSettings.LogLevel & PlayFabLogLevel.Warning) != 0)
            {
                UnityEngine.Debug.LogWarning(Util.timeStamp + " WARNING: " + Util.Format(text, args));
            }
        }

        public static void Error(string text, params object[] args)
        {
            if ((PlayFabSettings.LogLevel & PlayFabLogLevel.Error) != 0)
            {
                UnityEngine.Debug.LogError(Util.timeStamp + " ERROR: " + Util.Format(text, args));
            }
        }
    }
}

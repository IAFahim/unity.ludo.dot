/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// ----------------------------------------------------------------------------
// <copyright file="PhotonEditorUtils.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2011 Exit Games GmbH
// </copyright>
// <summary>
//   Unity Editor Utils
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;


namespace ExitGames.Client.Photon
{
    [InitializeOnLoad]
	public class PhotonEditorUtils
	{
        /// <summary>True if the ChatClient of the Photon Chat API is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasChat;
        /// <summary>True if the VoiceClient of the Photon Voice API is available. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasVoice;
        /// <summary>True if the PhotonEditorUtils checked the available products / APIs. If so, the editor may (e.g.) show additional options in settings.</summary>
        public static bool HasCheckedProducts;

	    static PhotonEditorUtils()
	    {
            HasVoice = Type.GetType("ExitGames.Client.Photon.Voice.VoiceClient, Assembly-CSharp") != null || Type.GetType("ExitGames.Client.Photon.Voice.VoiceClient, Assembly-CSharp-firstpass") != null;
            HasChat = Type.GetType("ExitGames.Client.Photon.Chat.ChatClient, Assembly-CSharp") != null || Type.GetType("ExitGames.Client.Photon.Chat.ChatClient, Assembly-CSharp-firstpass") != null;
            PhotonEditorUtils.HasCheckedProducts = true;
	    }


		public static void MountScriptingDefineSymbolToAllTargets(string defineSymbol)
		{
			foreach (BuildTargetGroup _group in Enum.GetValues(typeof(BuildTargetGroup)))
			{
				if (_group == BuildTargetGroup.Unknown) continue;
				
				List<string> _defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_group).Split(';').Select(d => d.Trim()).ToList();
				
				if (!_defineSymbols.Contains(defineSymbol))
				{
					_defineSymbols.Add(defineSymbol);
					PlayerSettings.SetScriptingDefineSymbolsForGroup(_group, string.Join(";", _defineSymbols.ToArray()));
				}
			}
		}
		
		public static void UnMountScriptingDefineSymbolToAllTargets(string defineSymbol)
		{
			foreach (BuildTargetGroup _group in Enum.GetValues(typeof(BuildTargetGroup)))
			{
				if (_group == BuildTargetGroup.Unknown) continue;
				
				List<string> _defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(_group).Split(';').Select(d => d.Trim()).ToList();
				
				if (_defineSymbols.Contains(defineSymbol))
				{
					_defineSymbols.Remove(defineSymbol);
					PlayerSettings.SetScriptingDefineSymbolsForGroup(_group, string.Join(";", _defineSymbols.ToArray()));
				}
			}
		}

	}
}

/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PunTeamsInspector.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  Custom inspector for PunTeams
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace ExitGames.UtilityScripts
{
	[CustomEditor(typeof(PunTeams))]
	public class PunTeamsInspector : Editor {


		Dictionary<PunTeams.Team, bool> _Foldouts ;

		public override void OnInspectorGUI()
		{
			if (_Foldouts==null)
			{
				_Foldouts = new Dictionary<PunTeams.Team, bool>();
			}

			if (PunTeams.PlayersPerTeam!=null)
			{
				foreach (KeyValuePair<PunTeams.Team,List<PhotonPlayer>> _pair in PunTeams.PlayersPerTeam)
				{	
					if (!_Foldouts.ContainsKey(_pair.Key))
					{
						_Foldouts[_pair.Key] = true;
					}

					_Foldouts[_pair.Key] =   EditorGUILayout.Foldout(_Foldouts[_pair.Key],"Team "+_pair.Key +" ("+_pair.Value.Count+")");

					if (_Foldouts[_pair.Key])
					{
						EditorGUI.indentLevel++;
						foreach(PhotonPlayer _player in _pair.Value)
						{
							EditorGUILayout.LabelField("",_player.ToString() + (PhotonNetwork.player==_player?" - You -":""));
						}
						EditorGUI.indentLevel--;
					}
				
				}
			}
		}
	}
}

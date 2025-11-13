/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextButtonTransition.cs" company="Exit Games GmbH">
// </copyright>
// <summary>
//  Use this on Button texts to have some color transition on the text as well without corrupting button's behaviour.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems;  
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{

	/// <summary>
	/// Use this on Button texts to have some color transition on the text as well without corrupting button's behaviour.
	/// </summary>
	[RequireComponent(typeof(Text))]
	public class TextButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		
		Text _text;

		public Color NormalColor= Color.white;
		public Color HoverColor = Color.black;

		public void Awake()
		{
			_text = GetComponent<Text>();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			_text.color = HoverColor;
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			_text.color = NormalColor; 
		}
	}
}

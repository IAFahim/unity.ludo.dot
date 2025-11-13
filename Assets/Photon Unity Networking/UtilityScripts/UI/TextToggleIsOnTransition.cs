/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextToggleIsOnTransition.cs" company="Exit Games GmbH">
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
	/// Use this on toggles texts to have some color transition on the text depending on the isOnState.
	/// </summary>
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {

		public Toggle toggle;

		Text _text;

		public Color NormalOnColor= Color.white;
		public Color NormalOffColor = Color.black;
		public Color HoverOnColor= Color.black;
		public Color HoverOffColor = Color.black;

		bool isHover;

		public void OnEnable()
		{
			_text = GetComponent<Text>();
		
			toggle.onValueChanged.AddListener(OnValueChanged);
		}

		public void OnDisable()
		{
			toggle.onValueChanged.RemoveListener(OnValueChanged);
		}

		public void OnValueChanged(bool isOn)
		{

				_text.color = isOn? (isHover?HoverOnColor:HoverOffColor) : (isHover?NormalOnColor:NormalOffColor) ;

		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			isHover = true;
			_text.color = toggle.isOn?HoverOnColor:HoverOffColor;
		}
		
		public void OnPointerExit(PointerEventData eventData)
		{
			isHover = false;
			_text.color = toggle.isOn?NormalOnColor:NormalOffColor;
		}

	}
}

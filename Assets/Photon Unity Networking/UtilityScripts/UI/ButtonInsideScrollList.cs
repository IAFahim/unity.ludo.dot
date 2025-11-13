/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ButtonInsideScrollList.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  Used on Buttons inside UI lists to prevent scrollRect parent to scroll when down on buttons.
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
	/// Button inside scroll list will stop scrolling ability of scrollRect container, so that when pressing down on a button and draggin up and down will not affect scrolling.
	/// this doesn't do anything if no scrollRect component found in Parent Hierarchy.
	/// </summary>
	public class ButtonInsideScrollList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		ScrollRect scrollRect;

		// Use this for initialization
		void Start () {
			scrollRect = GetComponentInParent<ScrollRect>();
		}

		#region IPointerDownHandler implementation
		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			if (scrollRect !=null)
			{
				scrollRect.StopMovement();
				scrollRect.enabled = false;
			}
		}
		#endregion

		#region IPointerUpHandler implementation

		void IPointerUpHandler.OnPointerUp (PointerEventData eventData)
		{
			if (scrollRect !=null && !scrollRect.enabled)
			{
				scrollRect.enabled = true;
			}
		}

		#endregion
	}
}

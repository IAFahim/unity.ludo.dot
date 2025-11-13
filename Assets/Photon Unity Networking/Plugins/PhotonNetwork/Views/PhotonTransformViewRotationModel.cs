/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformViewRotationModel.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2016 Exit Games GmbH
// </copyright>
// <summary>
//   Model class to synchronize rotations via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PhotonTransformViewRotationModel 
{
    public enum InterpolateOptions
    {
        Disabled,
        RotateTowards,
        Lerp,
    }

    public bool SynchronizeEnabled;

    public InterpolateOptions InterpolateOption = InterpolateOptions.RotateTowards;
    public float InterpolateRotateTowardsSpeed = 180;
    public float InterpolateLerpSpeed = 5;
}

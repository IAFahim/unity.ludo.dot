/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformViewPositionModel.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2016 Exit Games GmbH
// </copyright>
// <summary>
//   Model to synchronize position via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PhotonTransformViewPositionModel 
{
    public enum InterpolateOptions
    {
        Disabled,
        FixedSpeed,
        EstimatedSpeed,
        SynchronizeValues,
        //MoveTowardsComplex,
        Lerp,
    }

    public enum ExtrapolateOptions
    {
        Disabled,
        SynchronizeValues,
        EstimateSpeedAndTurn,
        FixedSpeed,
    }

    public bool SynchronizeEnabled;

    public bool TeleportEnabled = true;
    public float TeleportIfDistanceGreaterThan = 3f;

    public InterpolateOptions InterpolateOption = InterpolateOptions.EstimatedSpeed;
    public float InterpolateMoveTowardsSpeed = 1f;
    public float InterpolateLerpSpeed = 1f;
    public float InterpolateMoveTowardsAcceleration = 2;
    public float InterpolateMoveTowardsDeceleration = 2;
    public AnimationCurve InterpolateSpeedCurve = new AnimationCurve( new Keyframe[] { 
                                                                              new Keyframe( -1, 0, 0, Mathf.Infinity ), 
                                                                              new Keyframe( 0, 1, 0, 0 ), 
                                                                              new Keyframe( 1, 1, 0, 1 ), 
                                                                              new Keyframe( 4, 4, 1, 0 ) } );

    public ExtrapolateOptions ExtrapolateOption = ExtrapolateOptions.Disabled;
    public float ExtrapolateSpeed = 1f;
    public bool ExtrapolateIncludingRoundTripTime = true;
    public int ExtrapolateNumberOfStoredPositions = 1;

    //public bool DrawNetworkGizmo = true;
    //public Color NetworkGizmoColor = Color.red;
    //public ExitGames.Client.GUI.GizmoType NetworkGizmoType;
    //public float NetworkGizmoSize = 1f;

    //public bool DrawExtrapolatedGizmo = true;
    //public Color ExtrapolatedGizmoColor = Color.yellow;
    //public ExitGames.Client.GUI.GizmoType ExtrapolatedGizmoType;
    //public float ExtrapolatedGizmoSize = 1f;

    public bool DrawErrorGizmo = true;
}

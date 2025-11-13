/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/


#pragma warning disable 1587
/// \file
/// <summary>Reimplements a RPC Attribute, as it's no longer in all versions of the UnityEngine assembly.</summary>
#pragma warning restore 1587

using System;

/// <summary>Replacement for RPC attribute with different name. Used to flag methods as remote-callable.</summary>
public class PunRPC : Attribute
{
}

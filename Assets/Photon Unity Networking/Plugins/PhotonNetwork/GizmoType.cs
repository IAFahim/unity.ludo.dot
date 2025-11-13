/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

namespace ExitGames.Client.GUI
{
    public enum GizmoType
    {
        WireSphere,
        Sphere,
        WireCube,
        Cube,
    }

    public class GizmoTypeDrawer
    {
        public static void Draw( Vector3 center, GizmoType type, Color color, float size )
        {
            Gizmos.color = color;

            switch( type )
            {
            case GizmoType.Cube:
                Gizmos.DrawCube( center, Vector3.one * size );
                break;
            case GizmoType.Sphere:
                Gizmos.DrawSphere( center, size * 0.5f );
                break;
            case GizmoType.WireCube:
                Gizmos.DrawWireCube( center, Vector3.one * size );
                break;
            case GizmoType.WireSphere:
                Gizmos.DrawWireSphere( center, size * 0.5f );
                break;
            }
        }
    }
}

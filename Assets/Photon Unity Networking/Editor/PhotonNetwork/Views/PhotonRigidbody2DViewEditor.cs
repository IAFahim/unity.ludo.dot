/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (PhotonRigidbody2DView))]
public class PhotonRigidbody2DViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PhotonGUI.ContainerHeader("Options");

        Rect containerRect = PhotonGUI.ContainerBody(EditorGUIUtility.singleLineHeight*2 + 10);

        Rect propertyRect = new Rect(containerRect.xMin + 5, containerRect.yMin + 5, containerRect.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(propertyRect, serializedObject.FindProperty("m_SynchronizeVelocity"), new GUIContent("Synchronize Velocity"));

        propertyRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(propertyRect, serializedObject.FindProperty("m_SynchronizeAngularVelocity"), new GUIContent("Synchronize Angular Velocity"));

        serializedObject.ApplyModifiedProperties();
    }
}

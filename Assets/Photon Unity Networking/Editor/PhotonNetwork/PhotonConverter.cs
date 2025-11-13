/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// ----------------------------------------------------------------------------
// <copyright file="PhotonConverter.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2011 Exit Games GmbH
// </copyright>
// <summary>
//   Script to convert old RPC attributes into new RPC attributes.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------

#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 || UNITY_5_4_OR_NEWER
#define UNITY_MIN_5_3
#endif


using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PhotonConverter : Photon.MonoBehaviour
{
    public static List<string> GetScriptsInFolder(string folder)
    {
        List<string> scripts = new List<string>();

        try
        {
            scripts.AddRange(Directory.GetFiles(folder, "*.cs", SearchOption.AllDirectories));
            scripts.AddRange(Directory.GetFiles(folder, "*.js", SearchOption.AllDirectories));
            scripts.AddRange(Directory.GetFiles(folder, "*.boo", SearchOption.AllDirectories));
        }
        catch (System.Exception ex)
        {
            Debug.Log("Getting script list from folder " + folder + " failed. Exception:\n" + ex.ToString());
        }

        return scripts;
    }

    ///  default path: "Assets"
    public static void ConvertRpcAttribute(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }

        List<string> scripts = GetScriptsInFolder(path);
        foreach (string file in scripts)
        {
            string text = File.ReadAllText(file);
            string textCopy = text;
            if (file.EndsWith("PhotonConverter.cs"))
            {
                continue;
            }

            text = text.Replace("[RPC]", "[PunRPC]");
            text = text.Replace("@RPC", "@PunRPC");

            if (!text.Equals(textCopy))
            {
                File.WriteAllText(file, text);
                Debug.Log("Converted RPC to PunRPC in: " + file);
            }
        }
    }
}

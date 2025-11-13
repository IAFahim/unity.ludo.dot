/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SceneSetting
{
    public string sceneName;
    public int minViewId;
}

public class PunSceneSettings : ScriptableObject
{
    [SerializeField] public List<SceneSetting> MinViewIdPerScene = new List<SceneSetting>();

    private const string SceneSettingsFileName = "PunSceneSettingsFile.asset";

    // we use the path to PunSceneSettings.cs as path to create a scene settings file
    private static string punSceneSettingsCsPath;
    public static string PunSceneSettingsCsPath
    {
        get
        {
            if (!string.IsNullOrEmpty(punSceneSettingsCsPath))
            {
                return punSceneSettingsCsPath;
            }

            // Unity 4.3.4 does not yet have AssetDatabase.FindAssets(). Would be easier.
            var result = Directory.GetFiles(Application.dataPath, "PunSceneSettings.cs", SearchOption.AllDirectories);
            if (result.Length >= 1)
            {
                punSceneSettingsCsPath = Path.GetDirectoryName(result[0]);
                punSceneSettingsCsPath = punSceneSettingsCsPath.Replace('\\', '/');
                punSceneSettingsCsPath = punSceneSettingsCsPath.Replace(Application.dataPath, "Assets");

                // AssetDatabase paths have to use '/' and are relative to the project's folder. Always.
                punSceneSettingsCsPath = punSceneSettingsCsPath + "/" + SceneSettingsFileName;
            }

            return punSceneSettingsCsPath;
        }
    }


    private static PunSceneSettings instanceField;
    public static PunSceneSettings Instance
    {
        get
        {
            if (instanceField != null)
            {
                return instanceField;
            }

            instanceField = (PunSceneSettings)AssetDatabase.LoadAssetAtPath(PunSceneSettingsCsPath, typeof(PunSceneSettings));
            if (instanceField == null)
            {
                instanceField = ScriptableObject.CreateInstance<PunSceneSettings>();
                AssetDatabase.CreateAsset(instanceField, PunSceneSettingsCsPath);
            }

            return instanceField;
        }
    }


    public static int MinViewIdForScene(string scene)
    {
        if (string.IsNullOrEmpty(scene))
        {
            return 0;
        }

        PunSceneSettings pss = Instance;
        if (pss == null)
        {
            Debug.LogError("pss cant be null");
            return 0;
        }

        foreach (SceneSetting setting in pss.MinViewIdPerScene)
        {
            if (setting.sceneName.Equals(scene))
            {
                return setting.minViewId;
            }
        }
        return 0;
    }
}

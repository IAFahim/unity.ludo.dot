/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[InitializeOnLoad]
public class AndroidManifestManager {

	static AndroidManifestManager()
	{
#if UNITY_ANDROID
		if (PlayerSettings.applicationIdentifier != "com.playfab.sampleproj") {
			CustomizeManifest ();
		}
#endif
	}

	public static void CustomizeManifest()
	{
		string appId = PlayerSettings.applicationIdentifier;

		if(String.IsNullOrEmpty(appId) || appId == "com.Company.ProductName")
		{
			EditorUtility.DisplayDialog("Android Manifest Reminder", "Your project does not currently have a bundle identifier set. If you wish to publish on Android, you must manually edit your Android manifest at Assets/Plugins/Android/AndroindManifest.xml and replace all occurances of {APP_BUNDLE_ID} with your bundle identifier", "OK");
			return;
		}

		TextAsset manifestAsset = (TextAsset)AssetDatabase.LoadMainAssetAtPath ("Assets/Plugins/Android/AndroindManifest.xml");
		if (manifestAsset == null)
		{
			// No manifest to fix up
			return;
		}

		String manifestStr = manifestAsset.text;
		String fixedManifest = manifestStr.Replace ("{APP_BUNDLE_ID}", appId);
		if (fixedManifest == manifestStr)
		{
			// no changes made
			return;
		}

		AssetDatabase.RenameAsset ("Assets/Plugins/Android/AndroindManifest.xml", "Assets/Plugins/Android/AndroindManifest.xml.back");

		String path = Application.dataPath + "/Plugins/Android/AndroindManifest.xml";
		File.WriteAllText (path, fixedManifest);

		AssetDatabase.MoveAssetToTrash ("Assets/PlayFabSDK/Editor/AndroidManifestManager.cs");

		AssetDatabase.Refresh ();
	}
}

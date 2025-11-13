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

public class PlayFabPackager : MonoBehaviour {

	private static string[] SDKAssets = {
		"Assets/PlayFabSDK",
		"Assets/Plugins"
	};
    private static readonly string[] TEST_SCENES = {
        "Assets/PlayFabSDK/DemoScene/DemoScene.unity"
    };
    private const string BUILD_PATH = "C:/depot/sdks/UnitySDK/testBuilds/";

	[MenuItem ("PlayFab/Package SDK")]
	public static void PackagePlayFabSDK()
	{
		AssetDatabase.ExportPackage (SDKAssets, "../PlayFabClientSDK.unitypackage", ExportPackageOptions.Recurse);
	}

    private static void MkDir(string path)
    {
        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);
    }

    [MenuItem("PlayFab/Testing/AndroidTestBuild")]
    public static void MakeAndroidBuild()
    {
        string ANDROID_PACKAGE = System.IO.Path.Combine(BUILD_PATH, "PlayFabAndroid.apk");
        MkDir(BUILD_PATH);
        BuildPipeline.BuildPlayer(TEST_SCENES, ANDROID_PACKAGE, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("PlayFab/Testing/iPhoneTestBuild")]
    public static void MakeIPhoneBuild()
    {
        string IOS_PATH = System.IO.Path.Combine(BUILD_PATH, "PlayFabIOS");
        MkDir(BUILD_PATH);
        MkDir(IOS_PATH);
        BuildPipeline.BuildPlayer(TEST_SCENES, IOS_PATH, BuildTarget.iOS, BuildOptions.None);
    }

    [MenuItem("PlayFab/Testing/WinPhoneTestBuild")]
    public static void MakeWp8Build()
    {
        string WP8_PATH = System.IO.Path.Combine(BUILD_PATH, "PlayFabWP8");
        MkDir(BUILD_PATH);
        MkDir(WP8_PATH);
        BuildPipeline.BuildPlayer(TEST_SCENES, WP8_PATH, BuildTarget.WP8Player, BuildOptions.None);
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class Tools
{

    [MenuItem("Tools/CreateAssetBound")]
    public static void AssetBoundCreate()
    {
        AssetBundleWindow window = EditorWindow.GetWindow<AssetBundleWindow>();
        window.titleContent = new GUIContent("AssetBundle");
        window.Show();
    }

}

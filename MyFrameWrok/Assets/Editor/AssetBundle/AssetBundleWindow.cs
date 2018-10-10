using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary>
///  AssetBundle窗口
/// </summary>
public class AssetBundleWindow : EditorWindow
{
    private AssetBundleDAL dal;
    private List<AssetBundleEntitly> list;
    /// <summary>
    /// 是否选中
    /// </summary>
    private Dictionary<string, bool> dic;

    //打包标记
    private string[] arrTag = { "All", "Role", "Scene", "Effect", "Audio", "None" };
    private int tagIndex = 0;

    //打包平台设置
    private string[] arrayBuildTarget = { "Windows", "Android", "iOS" };
#if UNITY_STANDALONE_WIN
    private BuildTarget buildTarget = BuildTarget.StandaloneWindows;
    private int buildTargetIndex=0;
#elif UNITY_ANDROID
    private BuildTarget buildTarget = BuildTarget.Android;
    private int buildTargetIndex = 1;
#elif UNITY_IPHONE
    private BuildTarget buildTarget = BuildTarget.iOS;
    private int buildTargetIndex=1;
#endif
    private Vector2 pos;
    public AssetBundleWindow()
    {
        string path = Application.dataPath + @"\Editor\AssetBundle\AssetBundleConfig.xml";
        dal = new AssetBundleDAL(path);
        list = dal.GetList();

        dic = new Dictionary<string, bool>();
        foreach (var item in list)
        {
            dic[item.Key] = true;
        }
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    void OnGUI()
    {
        if (list == null || list.Count == 0)
        {
            return;
        }
        #region 按钮行
        GUILayout.BeginHorizontal("box");
        tagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (GUILayout.Button("选定Tag", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectTagCallBack;
        }
        buildTargetIndex = EditorGUILayout.Popup(buildTargetIndex, arrayBuildTarget, GUILayout.Width(100));
        if (GUILayout.Button("选定Target", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectTargetCallBack;
        }
        if (GUILayout.Button("打包AssetBundle", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnAssetBundleCallBack;
        }
        if (GUILayout.Button("清空AssetBundle", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCallBack;
        }
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("包名");
        GUILayout.Label("标记", GUILayout.Width(100));
        GUILayout.Label("保存路径", GUILayout.Width(200));
        GUILayout.Label("版本", GUILayout.Width(100));
        GUILayout.Label("大小", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        pos = EditorGUILayout.BeginScrollView(pos);
        for (int i = 0; i < list.Count; i++)
        {
            AssetBundleEntitly entity = list[i];
            GUILayout.BeginHorizontal("box");
            dic[list[i].Key] = EditorGUILayout.Toggle(dic[list[i].Key], GUILayout.Width(20));
            GUILayout.Label(entity.Name);
            GUILayout.Label(entity.Tag, GUILayout.Width(100));
            GUILayout.Label(entity.ToPath, GUILayout.Width(200));
            GUILayout.Label(entity.Version.ToString(), GUILayout.Width(100));
            GUILayout.Label(entity.Size.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();
            foreach (var item in entity.Path)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(40);
                GUILayout.Label(item);
                GUILayout.EndHorizontal();
            }

        }
        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 打包AssetBundle回调
    /// </summary>
    private void OnAssetBundleCallBack()
    {
        List<AssetBundleEntitly> needList = new List<AssetBundleEntitly>();
        for (int i = 0; i < list.Count; i++)
        {
            if (dic[list[i].Key])
            {
                needList.Add(list[i]);
            }
        }
        for (int i = 0; i < needList.Count; i++)
        {
            RunAssetBundBuild(needList[i]);
            Debug.LogFormat("正在打包：{0}/{1}", i, needList.Count);
        }
        Debug.Log("打包完成");
    }

    private void RunAssetBundBuild(AssetBundleEntitly entitly)
    {
        AssetBundleBuild[] arrBuild = new AssetBundleBuild[1];
        AssetBundleBuild build = new AssetBundleBuild();
        build.assetBundleName = string.Format("{0}.{1}", entitly.Name, entitly.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase) ? "Scene" : "AssetBundle");
        build.assetNames = entitly.Path.ToArray();
        arrBuild[0] = build;
        Debug.Log(entitly.ToPath);
        string toPath = Application.dataPath + "/../AssetBundles/" + arrayBuildTarget[buildTargetIndex] + entitly.ToPath;
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }
        BuildPipeline.BuildAssetBundles(toPath, arrBuild, BuildAssetBundleOptions.None, buildTarget);
    }



    /// <summary>
    /// 清空AssetBundle会回调
    /// </summary>
    private void OnClearAssetBundleCallBack()
    {
        string toPath = Application.dataPath + "/../AssetBundles";
        if (Directory.Exists(toPath))
        {
            Directory.Delete(toPath, true);
        }
    }
    /// <summary>
    /// 选定Taget回调
    /// </summary>
    private void OnSelectTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                buildTarget = BuildTarget.StandaloneWindows;
                break;
            case 1:
                buildTarget = BuildTarget.Android;
                break;
            case 2:
                buildTarget = BuildTarget.iOS;
                break;
        }
        Debug.LogFormat("选定的平台是：{0}，{1}", buildTargetIndex, arrayBuildTarget[buildTargetIndex]);
    }
    /// <summary>
    ///  选定Tag回调
    /// </summary>
    private void OnSelectTagCallBack()
    {
        //private string[] arrTag = { "All", "Role", "Scene", "Effect", "Audio", "None" };
        switch (tagIndex)
        {
            case 0:     //全选
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = true;
                }
                break;
            case 1: //Role
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Role");
                }
                break;
            case 2: //Scene
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Scene");
                }
                break;
            case 3: //Effect
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Effect");
                }
                break;
            case 4: //Audio
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = list[i].Tag.EndsWith("Audio");
                }
                break;
            case 5: //None
                for (int i = 0; i < list.Count; i++)
                {
                    dic[list[i].Key] = false;
                }
                break;
        }
        Debug.LogFormat("选定的标记是：{0},{1}", tagIndex, arrTag[tagIndex]);
    }
}

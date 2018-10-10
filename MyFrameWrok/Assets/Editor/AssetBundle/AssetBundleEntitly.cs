using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetBundle 实体
/// </summary>
public class AssetBundleEntitly
{
    /// <summary>
    /// 唯一键
    /// </summary>
    public string Key;

    /// <summary>
    /// 资源名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 资源标记
    /// </summary>
    public string Tag;

    /// <summary>
    /// 资源版本
    /// </summary>
    public int Version;

    /// <summary>
    /// 资源大小
    /// </summary>
    public long Size;

    /// <summary>
    /// 资源打包路径
    /// </summary>
    public string ToPath;

    private List<string> path = new List<string>();

    /// <summary>
    /// 资源路径
    /// </summary>
    public List<string> Path
    {
        get { return path; }
    }



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// AssetBundle ʵ��
/// </summary>
public class AssetBundleEntitly
{
    /// <summary>
    /// Ψһ��
    /// </summary>
    public string Key;

    /// <summary>
    /// ��Դ����
    /// </summary>
    public string Name;

    /// <summary>
    /// ��Դ���
    /// </summary>
    public string Tag;

    /// <summary>
    /// ��Դ�汾
    /// </summary>
    public int Version;

    /// <summary>
    /// ��Դ��С
    /// </summary>
    public long Size;

    /// <summary>
    /// ��Դ���·��
    /// </summary>
    public string ToPath;

    private List<string> path = new List<string>();

    /// <summary>
    /// ��Դ·��
    /// </summary>
    public List<string> Path
    {
        get { return path; }
    }



}

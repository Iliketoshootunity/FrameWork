using UnityEngine;
using System.Collections;
using System;

public class AssetBundleLoder : System.IDisposable
{

    private AssetBundle bundle;

    public AssetBundleLoder(string assetBundlepath)
    {    
        string fullPath = LocalFileMgr.localFilePath + assetBundlepath;
        byte[] buffer = LocalFileMgr.Instance.GetBuffer(fullPath);
        bundle = AssetBundle.CreateFromMemoryImmediate(buffer);
    }
    public void Dispose()
    {
        if (bundle == null) return;
        bundle.Unload(false);
    }

    public T Load<T>(string name) where T : UnityEngine.Object
    {
        if (bundle == null) return default(T);
        T t = bundle.LoadAsset<T>(name);
        return t;
    }


}

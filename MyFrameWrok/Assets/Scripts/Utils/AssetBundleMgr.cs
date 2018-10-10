using UnityEngine;
using System.Collections;

public class AssetBundleMgr : MonoBehaviour
{

    private static AssetBundleMgr _instance;

    public static AssetBundleMgr Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AssetBundleMgr();
            }
            return _instance;
        }
    }

    public GameObject Load(string path, string name)
    {
        using (AssetBundleLoder loader = new AssetBundleLoder(path))
        {
            return loader.Load<GameObject>(name);
        }
    }

    public GameObject LoadClone(string path, string name)
    {
        using (AssetBundleLoder loader = new AssetBundleLoder(path))
        {
            return Instantiate(loader.Load<GameObject>(name));
        }
    }
}

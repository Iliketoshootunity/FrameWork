using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        AssetBundleMgr.Instance.LoadClone(@"\Role\role_mainplayer.assetbundle", "role_mainplayer");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

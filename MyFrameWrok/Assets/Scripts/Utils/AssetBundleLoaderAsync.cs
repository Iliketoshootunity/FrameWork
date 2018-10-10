using UnityEngine;
using System.Collections;

public class AssetBundleLoaderAsync : MonoBehaviour {

    private string m_fullPath;
    private string m_name;

    public void Init(string fullPath,string name)
    {
        m_fullPath = fullPath;
        m_name = name;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator Load()
    {
        AssetBundle.
    }
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace DOUIFrame
{
    public class AssetInfo
    {
        private UnityEngine.Object obj;
        public Type AssetType { get; set; }
        public string Path { get; set; }
        public int RefCount { get; set; }
        public bool IsLoaded
        {
            get
            {
                return null != obj;
            }
        }
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (null == obj)
                {
                    resourcesLoad();
                }
                return obj;
            }
        }
        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> loaded)
        {
            while (true)
            {
                yield return null;
                if (null == obj)
                {
                    resourcesLoad();
                    yield return null;
                }
                else
                {
                    if (null != loaded)
                    {
                        loaded(obj);
                    }
                }
                yield break;
            }
        }
        private void resourcesLoad()
        {
            try
            {
                obj = Resources.Load(Path);
                if (null == obj)
                {
                    Debug.LogError("Error:null load obj");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> loaded)
        {
            return GetAsyncObject(loaded, null);
        }
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> loaded, Action<float> progress)
        {
            if (obj != null)
            {
                loaded(obj);
                yield break;
            }

            ResourceRequest resRequest = Resources.LoadAsync(Path);
            while (resRequest.progress < 0.9f)
            {
                if (null != progress)
                {
                    progress(resRequest.progress);
                }
                yield return null;
            }

            while (!resRequest.isDone)
            {
                if (null != progress)
                {
                    progress(resRequest.progress);
                }
                yield return null;
            }
            obj = resRequest.asset;
            if (null != obj)
            {
                loaded(obj);
            }
            yield return resRequest;
        }


    }

    public class ResManager : Singleton<ResManager>
    {
        private Dictionary<string, AssetInfo> dicAssetInfo = null;

        public override void Init()
        {
            dicAssetInfo = new Dictionary<string, AssetInfo>();
        }

        public UnityEngine.Object LoadInstance(string path)
        {
            UnityEngine.Object obj = Load(path);
            if (obj != null)
            {
                return instantiate(obj);
            }
            return null;
        }
        /// <summary>
        /// 协程加载并实例化
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是实例化的游戏物体</param>
        public void LoadCoroutineInstance(string path, Action<UnityEngine.Object> loaded)
        {
            LoadCoroutine(path, (obj) => { instantiate(obj, loaded); });
        }
        /// <summary>
        /// 异步加载并实例化
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是实例化的游戏物体</param>
        public void LoadAsyncInstance(string path, Action<UnityEngine.Object> loaded)
        {
            LoadAsync(path, (obj) => { instantiate(obj, loaded); });
        }
        /// <summary>
        /// 异步加载并实例化
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是实例化的游戏物体</param>
        /// <param name="progress">加载回调，参数是加载进度</param>
        public void LoadAsyncInstance(string path, Action<UnityEngine.Object> loaded, Action<float> progress)
        {
            LoadAsync(path, (obj) => { instantiate(obj, loaded); }, progress);
        }
        #region Load方法源
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public UnityEngine.Object Load(string path)
        {
            //if (String.IsNullOrEmpty(path))
            //{
            //    Debug.LogError("Error:null path name");
            //}
            //UnityEngine.Object obj = null;
            //return obj;
            AssetInfo assetInfo = GetAssetInfo(path);
            if (null != assetInfo)
            {
                return assetInfo.AssetObject;
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region LoadCoroutine 方法源
        /// <summary>
        /// 协程加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是Resource.load出来的物体</param>
        public void LoadCoroutine(string path, Action<UnityEngine.Object> loaded)
        {
            AssetInfo assetInfo = GetAssetInfo(path, loaded);
            if (null != assetInfo)
            {
                CoroutineController.Instance.StartCoroutine(assetInfo.GetCoroutineObject(loaded));
            }
            else
            {
                return;
            }
        }
        #endregion
        #region Load Async 方法源
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是Resource.load出来的物体</param>
        public void LoadAsync(string path, Action<UnityEngine.Object> loaded)
        {
            LoadAsync(path, loaded);
        }
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="loaded">加载回调，参数是Resource.load出来的物体</param>
        /// <param name="progress">加载回调，参数是加载进度</param>
        public void LoadAsync(string path, Action<UnityEngine.Object> loaded, Action<float> progress)
        {
            AssetInfo assetInfo = GetAssetInfo(path, loaded);
            if (null != assetInfo)
            {
                CoroutineController.Instance.StartCoroutine(assetInfo.GetAsyncObject(loaded, progress));
            }
        }
        #endregion
        #region 实例化
        private UnityEngine.Object instantiate(UnityEngine.Object _obj)
        {
            return instantiate(_obj, null);
        }
        private UnityEngine.Object instantiate(UnityEngine.Object obj, Action<UnityEngine.Object> loaded)
        {
            UnityEngine.Object retObj = null;
            if (obj != null)
            {
                retObj = MonoBehaviour.Instantiate(obj);
                if (retObj != null)
                {
                    if (loaded != null)
                    {
                        loaded(retObj);
                        return null;
                    }
                    return retObj;
                }
                else
                {
                    Debug.LogError("Error: null Instantiate _retObj.");
                }
            }
            else
            {
                Debug.LogError("Error: null Resources Load return _obj.");
            }
            return null;
        }
        #endregion
        #region GetAssetInfo
        public AssetInfo GetAssetInfo(string path)
        {
            return GetAssetInfo(path, null);
        }
        public AssetInfo GetAssetInfo(string path, Action<UnityEngine.Object> loaded)
        {
            if (String.IsNullOrEmpty(path))
            {
                Debug.LogError("Error:null path name");
                if (null != loaded)
                {
                    loaded(null);
                }
            }
            AssetInfo assetInfo = null;
            if (!dicAssetInfo.TryGetValue(path, out assetInfo))
            {
                assetInfo = new AssetInfo();
                assetInfo.Path = path;
                dicAssetInfo.Add(path, assetInfo);
            }
            assetInfo.RefCount++;
            return assetInfo;
        }
        #endregion
    }

}




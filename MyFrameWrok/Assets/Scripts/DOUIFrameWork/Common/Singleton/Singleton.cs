using UnityEngine;
using System.Collections;

namespace DOUIFrame
{
    public abstract class Singleton<T> where T : class, new()
    {
        protected static T _instance = null;

        public static T Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new T();
                }
                return _instance;
            }

        }
        protected Singleton()
        {
            if (null != _instance)
            {
                throw new SingletonException("this " + (typeof(T)).ToString() + "Singleton is not null");
            }
            Init();

        }
        public virtual void Init()
        {

        }
    }

    public abstract class DDOLSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance = null;

        public static T Instance
        {
            get
            {
                if (null == instance)
                {
                    GameObject go = GameObject.Find("DDOLGameObject");
                    if(null==go)
                    {
                        go = new GameObject("DDOLGameObject");
                        DontDestroyOnLoad(go);
                    }
                    instance = go.AddComponent<T>();
                }
                return instance;
                
            }
        }
        /// <summary>
        /// 程序退出时执行
        /// </summary>
        private void OnApplicationQuit()
        {
            instance = null;
        }
    }

}


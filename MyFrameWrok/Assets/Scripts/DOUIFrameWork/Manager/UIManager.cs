using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DOUIFrame
{
    public class UIManager : Singleton<UIManager>
    {

        /// <summary>
        /// UI的信息
        /// </summary>
        class UIInfoData
        {
            public EnumUIType UIType { get; private set; }
            public string Path { get; private set; }
            public object[] UIparams { get; private set; }

            public System.Type Scripts { get; private set; }
            public UIInfoData(EnumUIType uitype, string path, params object[] uiparams)
            {
                this.UIType = uitype;
                this.Path = path;
                this.UIparams = uiparams;
                this.Scripts = UIPathDefines.GetScirptsByType(this.UIType);
            }
        }

        /// <summary>
        /// 打开的UI
        /// </summary>
        private Dictionary<EnumUIType, GameObject> dicOpenUIs = null;
        /// <summary>
        /// 需要打开的UI.缓存池
        /// </summary>
        private Stack<UIInfoData> stackOpenUIs = null;

        public override void Init()
        {
            dicOpenUIs = new Dictionary<EnumUIType, GameObject>();
            stackOpenUIs = new Stack<UIInfoData>();
        }
        /// <summary>
        /// 获得UI的继承于BaseUI的类
        /// </summary>
        /// <typeparam name="T">继承BaseUI的泛型</typeparam>
        /// <param name="uitype">uid的类型</param>
        /// <returns></returns>
        public T GetUI<T>(EnumUIType uitype) where T : BaseUI
        {
            GameObject retObj = null;
            if (dicOpenUIs.TryGetValue(uitype, out retObj))
            {
                if (retObj != null)
                {
                    return retObj.GetComponent<T>();
                }
            }
            return null;
        }
        /// <summary>
        /// 获得UI的物体
        /// </summary>
        /// <param name="uitype"></param>
        /// <returns></returns>
        public GameObject GetUIObject(EnumUIType uitype)
        {
            GameObject retObj = null;
            if (!dicOpenUIs.TryGetValue(uitype, out retObj))
            {
                throw new System.Exception("not " + uitype);
            }
            return retObj;
        }
        #region 预加载UI
        public void PreLoadUI(EnumUIType uitype)
        {
            string path = UIPathDefines.GetPrefabsPathByType(uitype);
            Resources.Load(path);
        }
        public void PreLoadUI(EnumUIType[] uitypes)
        {
            for (int i = 0; i < uitypes.Length; i++)
            {
                PreLoadUI(uitypes[i]);
            }
        }
        #endregion

        #region 打开UI
        public void OpenUI(EnumUIType[] uiTypes)
        {
            OpenUI(false, uiTypes, null);
        }
        public void OpenUI(EnumUIType uiType, params object[] uiParams)
        {
            EnumUIType[] uiTypes = new EnumUIType[1];
            uiTypes[0] = uiType;
            OpenUI(false, uiTypes, uiParams);
        }
        public void OpenUICloseOthers(EnumUIType[] uiTypes)
        {
            OpenUI(true, uiTypes, null);
        }
        public void OpenUICloseOthers(EnumUIType uiType, params object[] uiParams)
        {
            EnumUIType[] uiTypes = new EnumUIType[1];
            uiTypes[0] = uiType;
            OpenUI(true, uiTypes, uiParams);
        }
        public void OpenUI(bool isCloseOther, EnumUIType[] uiTypes, params object[] uiParams)
        {
            //关闭其他UI
            if (isCloseOther)
            {
                CloseUIAll();
            }
            //push uiType to stackOpenUIs
            for (int i = 0; i < uiTypes.Length; i++)
            {
                EnumUIType uiType = uiTypes[i];
                if (!dicOpenUIs.ContainsKey(uiType))
                {
                    string path = UIPathDefines.GetPrefabsPathByType(uiType);
                    stackOpenUIs.Push(new UIInfoData(uiType, path, uiParams));
                }
            }
            //打开UI
            if (stackOpenUIs.Count > 0)
            {
                CoroutineController.Instance.StartCoroutine(asyncLoadData());
            }
        }

        private IEnumerator<int> asyncLoadData()
        {
            UIInfoData uiInfoData = null;
            UnityEngine.Object prefabObj = null;
            GameObject uiObj = null;
            if (stackOpenUIs != null && stackOpenUIs.Count > 0)
            {
                do
                {
                    uiInfoData = stackOpenUIs.Pop();
                    //prefabObj = Resources.Load(uiInfoData.Path);
                    prefabObj = ResManager.Instance.Load(uiInfoData.Path);
                    if (prefabObj != null)
                    {
                        uiObj = GameObject.Instantiate(prefabObj) as GameObject;
                        BaseUI baseUI = uiObj.GetComponent<BaseUI>();
                        if (baseUI == null)
                        {
                            baseUI = uiObj.AddComponent(uiInfoData.Scripts) as BaseUI;
                        }
                        if (null != baseUI)
                        {
                            baseUI.SetUIWhenOPening(uiInfoData.UIparams);
                        }
                        dicOpenUIs.Add(uiInfoData.UIType, uiObj);
                    }
                }
                while (stackOpenUIs.Count > 0);
            }
            yield return 0;
        }

        #endregion
        #region 关闭UI
        public void CloseUIAll()
        {
            List<EnumUIType> list = new List<EnumUIType>(dicOpenUIs.Keys);
            CloseUI(list.ToArray());
            list.Clear();
        }
        public void CloseUI(EnumUIType[] uiTypes)
        {
            for (int i = 0; i < uiTypes.Length; i++)
            {
                CloseUI(uiTypes[i]);
            }
        }
        public void CloseUI(EnumUIType uiType)
        {
            GameObject uiObj = GetUIObject(uiType);
            if (null == uiObj)
            {
                dicOpenUIs.Remove(uiType);
            }
            else
            {
                BaseUI baseUI = uiObj.GetComponent<BaseUI>();
                if (null == baseUI)
                {
                    GameObject.Destroy(uiObj);
                    dicOpenUIs.Remove(uiType);
                }
                else
                {
                    baseUI.StateChanged += CloseUIHandle;
                    baseUI.Relese();
                }
            }
        }
        public void CloseUIHandle(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                BaseUI baseUI = sender as BaseUI;
                dicOpenUIs.Remove(baseUI.GetUIType());
                baseUI.StateChanged -= CloseUIHandle;
            }

        }
        #endregion
    }



}

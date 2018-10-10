using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOUIFrame
{



    public class SceneManager : Singleton<SceneManager>
    {

        class SceneInfoData
        {
            public Type ScriptType { get; private set; }

            public string SceneName { get; private set; }

            public object[] UIParams { get; private set; }
            public SceneInfoData(Type ssriptType, string sceneName, params object[] uiParams)
            {
                this.ScriptType = ssriptType;
                this.SceneName = sceneName;
                this.UIParams = uiParams;
            }

        }

        private Dictionary<EnumSceneType, SceneInfoData> dicScneneInfo;

        private BaseScene currentScene;

        public BaseScene CurrentScene
        {
            get
            {
                return currentScene;
            }

            private set
            {
                if (currentScene != null) currentScene.Load();
                currentScene = value;
            }
        }



        private EnumSceneType lastScneneType;
        private EnumSceneType changeSceneType;


        private EnumUIType scneneOpenUIType = EnumUIType.None;
        private object[] uiParams;
        public EnumUIType ScneneOpenUIType
        {
            get
            {
                return scneneOpenUIType;
            }

            set
            {
                scneneOpenUIType = value;
            }
        }

        public object[] UiParams
        {
            get
            {
                return uiParams;
            }

            set
            {
                uiParams = value;
            }
        }
        public override void Init()
        {
            dicScneneInfo = new Dictionary<EnumSceneType, SceneInfoData>();
        }

        public void RegisterAll()
        {
            Register(EnumSceneType.MainScene, typeof(BaseScene), "mainScnene", null);
            Register(EnumSceneType.LoginScene, typeof(BaseScene), "LoginScene", null);
        }

        public void Register(EnumSceneType scnenId, Type ssriptType, string sceneName, params object[] sceneParams)
        {
            SceneInfoData info = new SceneInfoData(ssriptType, sceneName, sceneParams);
            if (IsRegister(scnenId))
            {
                dicScneneInfo[scnenId] = info;
            }
            else
            {
                dicScneneInfo.Add(scnenId, info);
            }
        }
        public void UnRegister(EnumSceneType scnenId)
        {
            if (dicScneneInfo != null && IsRegister(scnenId))
            {
                dicScneneInfo.Remove(scnenId);
            }
        }

        public bool IsRegister(EnumSceneType scnenId)
        {
            return dicScneneInfo.ContainsKey(scnenId);
        }

        public string GetSceneName(EnumSceneType sceneId)
        {
            if (IsRegister(sceneId))
            {
                SceneInfoData info = dicScneneInfo[sceneId];
                return info.SceneName;
            }
            return "";
         
        }

        public BaseScene GetScene(EnumSceneType scnenId)
        {
            Debug.Log("Get  Scene: " + scnenId);
            SceneInfoData info = dicScneneInfo[scnenId];
            if (info == null || info.ScriptType == null) return null;
            return System.Activator.CreateInstance(info.ScriptType) as BaseScene;
        }


        public void ChangeScneneDirect(EnumSceneType sceneType, EnumUIType uiType, params object[] uiParams)
        {
            lastScneneType = changeSceneType;
            changeSceneType = sceneType;
            ScneneOpenUIType = uiType;
            this.UiParams = uiParams;
            if (lastScneneType == sceneType)
            {
                if (ScneneOpenUIType != EnumUIType.None)
                {
                    UIManager.Instance.OpenUI(ScneneOpenUIType, uiParams);
                    ScneneOpenUIType = EnumUIType.None;
                }
            }
            else
            {
                ChangeScneneDirect(sceneType);
            }
        }

        public void ChangeScneneDirect(EnumSceneType sceneType)
        {
            UIManager.Instance.CloseUIAll();
            if (CurrentScene != null)
            {
                CurrentScene.Release();
            }
            lastScneneType = changeSceneType;
            changeSceneType = sceneType;
            string sceneName = GetSceneName(sceneType);
            CoroutineController.Instance.StartCoroutine(AysncLoadScene(sceneName));
        }

        private IEnumerator AysncLoadScene(string scneneName)
        {
            AsyncOperation opr = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scneneName);
            yield return opr;
            if (ScneneOpenUIType != EnumUIType.None)
            {
                UIManager.Instance.OpenUI(ScneneOpenUIType, UiParams);
            }
            CurrentScene = GetScene(changeSceneType);
        }
    }
}
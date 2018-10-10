using UnityEngine;
using System.Collections;

namespace DOUIFrame
{
    public abstract class BaseUI : MonoBehaviour
    {
        #region 缓存GameObject和Transform
        private GameObject cacheGameObject;
        /// <summary>
        /// 缓存GameObject
        /// </summary>
        public GameObject CacheGameObject
        {
            get
            {
                if (null == cacheGameObject)
                    cacheGameObject = this.gameObject;
                return cacheGameObject;
            }
        }
        /// <summary>
        /// 缓存Transform
        /// </summary>
        public Transform CacheTransfrom
        {
            get
            {
                if (null == cacheTransfrom)
                    cacheTransfrom = this.transform;
                return cacheTransfrom;
            }

        }



        private Transform cacheTransfrom;
        #endregion
        #region EnumObjectState 和 EnumUIType
        public event StateChangeEvent StateChanged;
        private EnumObjectState state = EnumObjectState.None;
        /// <summary>
        /// UI状态
        /// </summary>
        public EnumObjectState State
        {
            protected get
            {
                return state;
            }

            set
            {
                EnumObjectState oldSate = this.state;
                state = value;
                if (null != StateChanged)
                {
                    StateChanged(this, this.state, oldSate);
                }

            }
        }
        /// <summary>
        /// UI类型
        /// </summary>
        /// <returns></returns>
        public abstract EnumUIType GetUIType();
        #endregion

        #region Unity 生命周期
        void Awake()
        {
            this.State = EnumObjectState.Initial;
            OnAwake();
        }
        // Use this for initialization
        void Start()
        {
            OnStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.State == EnumObjectState.Ready)
            {
                OnUpdate(Time.deltaTime);
            }

        }

        private void OnDestroy()
        {
            this.State = EnumObjectState.None;
        }

        /// <summary>
        /// 对象初始化1
        /// </summary>
        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
            this.OnPlayOpenUIAudio();
        }
        /// <summary>
        /// 对象初始化2
        /// </summary>
        protected virtual void OnStart() { }
        /// <summary>
        /// 每帧更新
        /// </summary>
        /// <param name="deltaTime"></param>
        protected virtual void OnUpdate(float deltaTime) { }


        #endregion
        /// <summary>
        /// 打开UI时设置UI
        /// </summary>
        /// <param name="uiParams"></param>
        public void SetUIWhenOPening(params object[] uiParams)
        {
            SetUI(uiParams);
            CoroutineController.Instance.StartCoroutine(asyncLoadData());
        }

        public void Relese()
        {
            this.State = EnumObjectState.Closing;
            OnRelese();
            GameObject.Destroy(this.CacheGameObject);

        }

        #region 内部继承方法
        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="uiParams"></param>
        protected virtual void SetUI(params object[] uiParams)
        {
            this.State = EnumObjectState.Loading;
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        protected virtual void OnRelese()
        {
            this.state = EnumObjectState.None;
            this.OnPlayCloseUIAudio();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        protected virtual void OnLoadData()
        {

        }
        /// <summary>
        /// 打开UI的声音
        /// </summary>
        protected virtual void OnPlayOpenUIAudio()
        {

        }
        /// <summary>
        /// 关闭UI的声音
        /// </summary>
        protected virtual void OnPlayCloseUIAudio()
        {

        }

        protected virtual void SetDepthToTop()
        {

        }
        #endregion

        /// <summary>
        /// 异步加载数据
        /// </summary>
        /// <returns></returns>
        private IEnumerator asyncLoadData()
        {
            yield return new WaitForSeconds(0);
            if (this.State == EnumObjectState.Loading)
            {
                OnLoadData();
                this.State = EnumObjectState.Ready;
            }
        }
    }
}



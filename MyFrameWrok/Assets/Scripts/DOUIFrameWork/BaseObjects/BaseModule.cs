using UnityEngine;
using System.Collections;

namespace DOUIFrame
{
    public class BaseModule 
    {
        public enum EnumRegisterMode
        {
            NotRegister,
            AutoRegister,
            AlreadyRegister
        }

        private EnumObjectState state = EnumObjectState.Initial;

        public EnumObjectState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state == value)
                    return;
                EnumObjectState oldState = state;
                state = value;
                if (StateChange != null)
                {
                    StateChange(this, state, oldState);
                }

            }
        }

        public event StateChangeEvent StateChange;

        protected virtual void OnStateChanged(EnumObjectState newState, EnumObjectState oldState)
        {

        }
        private EnumRegisterMode registerMode = EnumRegisterMode.NotRegister;

        public bool AutoRegister
        {
            get
            {
                return registerMode == EnumRegisterMode.NotRegister ? false : true;
            }
            set
            {
                if (registerMode == EnumRegisterMode.NotRegister || registerMode == EnumRegisterMode.AutoRegister)
                {
                    registerMode = value ? EnumRegisterMode.AutoRegister : EnumRegisterMode.NotRegister;
                }
            }
        }
        public bool HasReigister
        {
            get
            {
                return registerMode == EnumRegisterMode.AlreadyRegister;
            }
        }
        /// <summary>
        /// 加载
        /// </summary>
        public void Load()
        {
            if (state != EnumObjectState.Initial) return;
            State = EnumObjectState.Loading;
            OnLoad();
            if (registerMode == EnumRegisterMode.AutoRegister)
            {
                ModuleManager.Instance.Register(this);
                registerMode = EnumRegisterMode.AlreadyRegister;
            }
            State = EnumObjectState.Ready;
        }
        /// <summary>
        /// 加载
        /// </summary>
        protected virtual void OnLoad()
        {

        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            if (State != EnumObjectState.Disabled)
            {
                State = EnumObjectState.Disabled;
                if (registerMode == EnumRegisterMode.AlreadyRegister)
                {
                    ModuleManager.Instance.UnRegister(this);
                    registerMode = EnumRegisterMode.AutoRegister;
                }
                OnRelease();
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        protected virtual void OnRelease()
        {

        }
    }
}

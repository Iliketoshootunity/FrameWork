using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DOUIFrame
{
    /// <summary>
    /// 角色
    /// </summary>
    public class BaseActor : IDynamicProperty
    {
        //角色的Load Realse 类似于BaseModule的处理逻辑
        //动画，技能，状态机类似于PropertyItem的处理方法

        public int ID { get; set; }

        protected Dictionary<int, PropertyItem> dicProperty;
        public EnumActorType ActorType { get; set; }

        private BaseScene currentScene;
        public virtual  BaseScene CurrentScene
        {
            get
            {
                //加载场景的逻辑
                return currentScene;
            }

            set
            {
                currentScene = value;
            }
        }

        public PropertyChangedHandle PropertyChanged;


        public BaseActor()
        {

        }

        public virtual void Load()
        {

        }

        public virtual void Release()
        {

        }

        #region Property
        public virtual void AddProperty(EnumPropertyType propertyType, object content)
        {
            PropertyItem pro = new PropertyItem((int)propertyType, content);
            AddProperty(pro);

        }
        public virtual void AddProperty(int id, object content)
        {
            PropertyItem pro = new PropertyItem(id, content);
            AddProperty(pro);
        }

        public virtual void AddProperty(PropertyItem property)
        {
            if (dicProperty == null)
            {
                dicProperty = new Dictionary<int, PropertyItem>();
            }
            if (dicProperty.ContainsValue(property))
            {
                RemoveProperty(property.ID);
            }
            dicProperty.Add(property.ID, property);
            property.Owner = this;
        }
        public virtual void RemoveProperty(EnumPropertyType propertyType)
        {
            RemoveProperty((int)propertyType);
        }
        public virtual void RemoveProperty(int id)
        {
            if (dicProperty != null && dicProperty.ContainsKey(id))
                dicProperty.Remove(id);
        }

        public virtual void ClearProperty()
        {
            if (null != dicProperty)
            {
                dicProperty.Clear();
                dicProperty = null;
            }
        }

        public virtual PropertyItem GetProperty(EnumPropertyType propertyType)
        {
            return GetProperty((int)propertyType);
        }
        #region IDynamicProperty
        public virtual PropertyItem GetProperty(int id)
        {
            if (null == dicProperty)
                return null;
            if (dicProperty.ContainsKey(id))
                return dicProperty[id];
            return null;
        }

        public virtual void DOChangeProperty(int id, object oldValue, object newValue)
        {
            OnPropertyChanged(id, oldValue, newValue);
            if (null != PropertyChanged)
                PropertyChanged(this, id, oldValue, newValue);
        }

        #endregion
        protected virtual void OnPropertyChanged(int id, object oldValue, object newValue)
        {

        }
        #endregion
    }
}


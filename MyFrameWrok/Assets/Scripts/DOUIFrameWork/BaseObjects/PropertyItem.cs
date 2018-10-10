using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DOUIFrame
{
    /// <summary>
    /// 属性
    /// </summary>
    public class PropertyItem
    {

        public int ID { get; set; }

        public object Content
        {
            get
            {
                return content;
            }

            set
            {
                if (value == content)
                {
                    return;
                }
                object old = this.content;
                setContent(value);
                if (Owner != null)
                {
                    Owner.DOChangeProperty(ID, old, this.content);
                }
            }
        }

        private object rawContent;
        private bool canRandom;
        private float floatRandom;
        private int intRandom;
        private object content;
        private Type propertyType;
        //拥有者
        public IDynamicProperty Owner;

        public PropertyItem(int id, object content)
        {
            this.ID = id;
            propertyType = content.GetType();
            if (propertyType == typeof(System.Int32) || propertyType == typeof(System.Single))
            {
                canRandom = true;
            }
            setContent(content);
        }
        public object GetContent()
        {
            object c = null;
            if (canRandom == true)
            {
                if (propertyType == typeof(System.Int32))
                {
                    c = (int)this.Content - intRandom;
                    if (c != rawContent)
                    {
                        Debug.LogError("数据有问题");
                    }

                }
                else if (propertyType == typeof(System.Single))
                {
                    c = (float)this.Content - floatRandom;
                    if (c != rawContent)
                    {
                        Debug.LogError("数据有问题");
                    }
                }

            }
            else
            {
                c = this.Content;
            }
            return c;
        }
        private void setContent(object content)
        {
            this.rawContent = content;
            if (canRandom == true)
            {
                if (propertyType == typeof(System.Int32))
                {
                    intRandom = UnityEngine.Random.Range(0, 100);
                    this.Content = (int)content + intRandom;
                }
                else if (propertyType == typeof(System.Single))
                {
                    floatRandom = UnityEngine.Random.Range(0f, 100f);
                    this.Content = (int)content + floatRandom;
                }
            }
            else
            {
                this.Content = content;
            }
        }


    }

}


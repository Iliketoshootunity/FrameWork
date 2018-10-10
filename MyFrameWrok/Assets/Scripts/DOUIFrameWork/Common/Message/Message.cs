using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace DOUIFrame
{
    public class Message : IEnumerable<KeyValuePair<string, object>>
    {
        private Dictionary<string, object> dicDatas = null;

        public object this[string key]
        {
            get
            {
                if (null == dicDatas || !dicDatas.ContainsKey(key))
                    return null;
                return dicDatas[key];
            }
            set
            {
                if (null == dicDatas)
                {
                    dicDatas = new Dictionary<string, object>();
                }
                if (dicDatas.ContainsKey(key))
                {
                    dicDatas[key] = value;
                }
                else
                {
                    dicDatas.Add(key, value);
                }
            }
        }

        public string Name { get; set; }
        public object Sender { get; set; }
        public object Content { get; set; }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            if (null == dicDatas)
                yield break;
            foreach (KeyValuePair<string, object> kvp in dicDatas)
            {
                yield return kvp;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dicDatas.GetEnumerator();
        }

        public Message(string name, object sender)
        {
            this.Name = name;
            this.Sender = sender;
        }
        public Message(string name, object sender, object content)
        {
            this.Name = name;
            this.Sender = sender;
            this.Content = content;
        }
        public Message(string name, object sender, object content, params object[] dicParams)
        {
            this.Name = name;
            this.Sender = sender;
            this.Content = content;
            if (dicParams.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (object dicParam in dicParams)
                {
                    foreach (KeyValuePair<string, object> kvp in dicParam as Dictionary<string, object>)
                    {
                        //dicDatas[kvp.Key] = kvp.Value;  //error
                        this[kvp.Key] = kvp.Value;
                    }
                }
            }
        }
        public void Add(string key, object value)
        {
            this[key] = value;
        }
        public void Remove(string key)
        {
            if (null != dicDatas && dicDatas.ContainsKey(key))
            {
                dicDatas.Remove(key);
            }
        }
        public void Send()
        {
            MessageCenter.Instance.SendMessage(this);
        }
    }
}



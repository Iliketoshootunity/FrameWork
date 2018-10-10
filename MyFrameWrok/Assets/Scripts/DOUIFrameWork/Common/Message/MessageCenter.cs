using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DOUIFrame
{
    public class MessageCenter : Singleton<MessageCenter>
    {
        private Dictionary<string, List<MessageEvent>> dicMessageEvents = null;

        public override void Init()
        {
            dicMessageEvents = new Dictionary<string, List<MessageEvent>>();
        }

        public void AddListen(string messageName, MessageEvent messageEvent)
        {
            Debug.Log("AddListener Name : " + messageName);
            List<MessageEvent> list = null;
            if (dicMessageEvents.TryGetValue(messageName, out list))
            {
                list = dicMessageEvents[messageName];
            }
            else
            {
                list = new List<MessageEvent>();
                dicMessageEvents.Add(messageName, list);
            }
            if (!list.Contains(messageEvent))
            {
                list.Add(messageEvent);
            }

        }
        public void RemoveListen(string messageName, MessageEvent messageEvent)
        {
            Debug.Log("RemoveListener Name : " + messageName);
            List<MessageEvent> list = null;
            if (dicMessageEvents.TryGetValue(messageName, out list))
            {
                list = dicMessageEvents[messageName];
                if (list.Contains(messageEvent))
                {
                    list.Remove(messageEvent);
                }
                if (list.Count <= 0)
                {
                    dicMessageEvents.Remove(messageName);
                }
            }
        }
        public void RemoveAllListener()
        {
            dicMessageEvents.Clear();
        }
        public void SendMessage(Message message)
        {
            DOMessageDispatcher(message);
        }
        public void SendMessage(string name, object sender)
        {
            SendMessage(new Message(name, sender));
        }
        public void SenndMessage(string name, object sender, object content)
        {
            SendMessage(new Message(name, sender, content));
        }
        private void DOMessageDispatcher(Message message)
        {
            if (dicMessageEvents == null || !dicMessageEvents.ContainsKey(message.Name))
                return;
            List<MessageEvent> list = dicMessageEvents[message.Name];
            for (int i = 0; i < list.Count; i++)
            {
                MessageEvent temp = list[i];
                if (null != temp)
                {
                    temp(message);
                }
            }
        }
    }
}



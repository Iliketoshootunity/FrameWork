using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DOUIFrame
{
    /// <summary>
    /// 类似于BaseModule
    /// </summary>
    public class BaseScene : BaseModule
    {
        //触发器 相机 load realse 
        //pveScene PvpScene :FightScene 来创建小怪
        //可以管理自己需要的属性


        protected List<BaseActor> actorList = null;

        public void AddActor(BaseActor actor)
        {
            if (actorList == null)
            {
                actorList = new List<BaseActor>();
            }
            if (!actorList.Contains(actor))
            {
                actor.CurrentScene = this;
                actor.PropertyChanged += actorPropertyChanged;
                actor.Load();
                actorList.Add(actor);

            }
        }

        public void RemoveActor(BaseActor actor)
        {
            if (actorList != null && actorList.Contains(actor))
            {
                actor.CurrentScene = null;
                actor.PropertyChanged -= actorPropertyChanged;
                actor.Release();
                actorList.Remove(actor);
            }
        }
        public BaseActor GetActorByID(int id)
        {
            if (actorList != null && actorList.Count > 0)
            {
                return actorList.Find((x) => x.ID == id);
            }
            else
            {
                return null;
            }
        }
        protected virtual void actorPropertyChanged(BaseActor actor, int id, object oldValue, object newValue)
        {

        }
    }
}


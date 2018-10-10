using System;
using System.Collections;
using System.Collections.Generic;


namespace DOUIFrame
{
    public class ModuleManager : Singleton<ModuleManager>
    {
        private Dictionary<string, BaseModule> dicModules = null;
        public override void Init()
        {
            dicModules = new Dictionary<string, BaseModule>();
        }
   
        
        public BaseModule GetModule(string key)
        {
            BaseModule temp = null;
            if (dicModules.TryGetValue(key, out temp))
            {
                return temp;
            }
     
            return null;
        }
        public T GetModule<T>() where T : BaseModule
        {
            Type t = typeof(T);
            if (dicModules.ContainsKey(t.ToString()))
            {
                return dicModules[t.ToString()] as T;
            }
            return null;
        }
        public void RegisterAll()
        {
            //LoadModule(typeof(MainModule));
        }
        public BaseModule LoadModule(Type moduleType)
        {
            BaseModule bm = System.Activator.CreateInstance(moduleType) as BaseModule;
            bm.AutoRegister = true;
            bm.Load();
            return bm;
        }
        public void Register(BaseModule module)
        {
            Type t = module.GetType();
            Register(t.ToString(), module);
        }
        public void Register(string key, BaseModule module)
        {
            if (!dicModules.ContainsKey(key))
            {
                dicModules.Add(key, module);
            }

        }
        public void UnRegister(BaseModule module)
        {
            Type t = module.GetType();
            UnRegister(t.ToString());
        }
        public void UnRegister(string key)
        {
            if (dicModules.ContainsKey(key))
            {
                BaseModule module = dicModules[key];
                //module.Release();
                dicModules.Remove(key);
                module = null;
            }
        }

    }

}

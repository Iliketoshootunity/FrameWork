using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DOUIFrame
{
    public interface IDynamicProperty
    {
        //更改属性
        void DOChangeProperty(int id, object oldValue, object newValue);
        PropertyItem GetProperty(int id);
    }
}


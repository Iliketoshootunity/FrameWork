using UnityEngine;
using System.Collections;
using System;

namespace DOUIFrame
{
    public class SingletonException : Exception
    {

        public SingletonException(string msg) : base(msg)
        {

        }
    }
}


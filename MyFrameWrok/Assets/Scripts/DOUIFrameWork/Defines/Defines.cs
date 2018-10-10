using UnityEngine;
using System.Collections;

namespace DOUIFrame
{
    #region 全局委托
    public delegate void StateChangeEvent(object sender, EnumObjectState newState, EnumObjectState oldState);
    public delegate void MessageEvent(Message message);
    public delegate void OnTouchEventHandle(GameObject listener, object args, params object[] paramss);
    public delegate void PropertyChangedHandle(BaseActor actor, int id, object oldValue, object newValue);
    #endregion
    #region 全局枚举
    public enum EnumObjectState
    {
        None,
        Initial,
        Loading,
        Ready,
        Disabled,
        Closing
    }
    public enum EnumUIType : int
    {
        None = 0,
        TestOne,
        TestTwo,
        MainUI
    }

    public enum EnumSceneType
    {
        None = 0,
        StartGame,
        LoginScene,
        MainScene,
        CopyScene,
        PVPScene
    }

    public enum EnumTouchEventType
    {
        OnClickHandle,
        OnDoubleClickHandle,
        OnDownHandle,
        OnUpHandle,
        OnEnterHandle,
        OnExitHandle,
        OnSelectHandle,
        OnUpdateSelectHandle,
        OnDeSelectHandle,
        OnDragHandle,
        OnDragEndHandle,
        OnDropHandle,
        OnScrollHandle,
        OnMoveHandle,
    }
    public enum EnumActorType
    {
        None = 0,
        Role,
        Monster,
        Npc
    }

    public enum EnumPropertyType : int
    {
        RoleName = 1,
        Sex,
        RoleID,
        Gold,
        Coin,
        Level,
        Exp,

        AttackSpeed,
        HP,
        HPMAX,
        Attack
    }

    #endregion
    public class UIPathDefines
    {
        /// <summary>
        /// UI预设
        /// </summary>
        public const string UI_PREFAB = "Prefabs/";
        /// <summary>
        /// UI小控件预设
        /// </summary>

        public const string UI_CONTROLS_PREFAB = "UIPrefab/Control/";
        /// <summary>
        /// UI子页面预设
        /// </summary>
        public const string UI_SUBUI_PREFAB = "UIPrefab/SunUI/";
        /// <summary>
        /// icon路径
        /// </summary>
        public const string UI_ICON_PATH = "UI/Icon/";

        /// <summary>
        /// 根据UI的类型获取UI的路径
        /// </summary>
        /// <param name="uitype"></param>
        /// <returns></returns>
        public static string GetPrefabsPathByType(EnumUIType uitype)
        {
            string path = string.Empty;
            switch (uitype)
            {
                case EnumUIType.TestOne:
                    path = UI_PREFAB + "TestUIOne";
                    break;
                case EnumUIType.TestTwo:
                    path = UI_PREFAB + "TestUITwo";
                    break;
                case EnumUIType.MainUI:
                    path = UI_PREFAB + "MainUI";
                    break;
                default:
                    Debug.Log("Not Find EnumUIType " + uitype.ToString());
                    break;
            }
            return path;
        }

        public static System.Type GetScirptsByType(EnumUIType uitype)
        {
            System.Type scriptType = null;
            switch (uitype)
            {
                case EnumUIType.TestOne:
                    break;
                case EnumUIType.TestTwo:
                    break;
                case EnumUIType.MainUI:
                    //scriptType = typeof(MainUI);
                    break;
                default:
                    Debug.Log("Not find EnumUIType type:" + uitype.ToString());
                    break;
            }
            return scriptType;
        }
    }

    public class Defines : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}



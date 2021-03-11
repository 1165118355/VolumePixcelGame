using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox.Widgets
{
    class WidgetFactory
    {
        static public WidgetFactory m_Instance = new WidgetFactory();
        List<WidgetClassBase> m_WidgetClass = new List<WidgetClassBase>();
        static public WidgetFactory Instance{ get { return m_Instance; } }

        public void registerWidget<T>() where T : Widget, new()
        {
            m_WidgetClass.Add(new WidgetClass<T>());
        }

        public GameObject createWidget<T> () where T : class
        {
            string type = typeof(T).ToString();
            foreach(var i in m_WidgetClass)
            {
                if(i.m_ClassName == type)
                {
                    return i.create();
                }
            }
            return null;
        }

    }

    class WidgetClassBase
    {
        public string m_ClassName;

        public virtual GameObject create(GameObject parent = null)
        {
            throw new NotImplementedException();
        }
    }

    class WidgetClass<T> : WidgetClassBase where T : Widget, new()
    {
        public WidgetClass()
        {
            m_ClassName = typeof(T).ToString();
        }
        public override GameObject create(GameObject parent = null)
        {
            GameObject obj = new GameObject();
            T widget = obj.AddComponent<T>();
            widget.init(obj);
            if(parent)
            {
                obj.transform.SetParent(parent.transform);
            }
            return obj;
        }
    }
}

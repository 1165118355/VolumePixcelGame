using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace WaterBox.Widgets
{
    class Actions:Widget
    {
        List<GameObject> m_ActionsList = new List<GameObject>();
        public UnityEvent onActionClicked = new UnityEvent();
        public override void init(GameObject obj)
        {
            base.init(obj);

            VerticalLayoutGroup vbox = obj.AddComponent<VerticalLayoutGroup>();
            vbox.childForceExpandWidth = true;
        }

        public GameObject addAction(string actionName)
        {
            GameObject actionObj = WidgetFactory.Instance.createWidget<Action>();
            m_ActionsList.Add(actionObj);
            actionObj.transform.parent = m_gameObject.transform;
            return actionObj;
        }
    }
}

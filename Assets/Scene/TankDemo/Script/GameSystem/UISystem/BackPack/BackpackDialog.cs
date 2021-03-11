using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox.Widgets
{
    class BackpackDialog : Dialog
    {
        Vector2Int m_Size;
        GameObject m_BackPack;
        public static GameObject createBackpackDialog(Vector2Int size = new Vector2Int())
        {
            GameObject obj = WidgetFactory.Instance.createWidget<BackpackDialog>();
            obj.GetComponent<BackpackDialog>().m_Size = size;
            return obj;
        }  
        public override void init(GameObject obj)
        {
            base.init(obj);
            VerticalLayoutGroup layout = m_Content.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandHeight = true;
            layout.childForceExpandWidth = true;
            m_BackPack = BackpackUI.createBackpack(m_Size);
            m_BackPack.transform.SetParent(m_Content.transform);
            m_Rect.localPosition = new Vector2(0, 0);
            m_Rect.sizeDelta = new Vector2(250, 250);
        }
    }
}

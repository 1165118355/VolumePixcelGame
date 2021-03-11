using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox.Widgets
{
    class Dialog :Widget
    {
        int m_TitleHeight = 25;
        public GameObject m_Content;
        GameObject m_CloseB;
        RectTransform m_ContentRect;
        RectTransform m_CloseRect;
        Button m_CloseButton;
        public Dialog()
        {
            m_Type = WidgetType.TYPE_DIALOG;
        }
        public override void init(GameObject obj)
        {
            base.init(obj);
            m_gameObject.name = "DialogUI";

            m_Content = new GameObject();
            m_CloseB = new GameObject();

            m_Content.name = "DialogUIContent";
            m_CloseB.name = "DialogUIClose";
            m_ContentRect = m_Content.AddComponent<RectTransform>();
            m_CloseRect = m_CloseB.AddComponent<RectTransform>();
            m_Content.AddComponent<CanvasRenderer>();
            m_CloseB.AddComponent<CanvasRenderer>();
            m_Content.AddComponent<Image>();
            m_CloseB.AddComponent<Image>().sprite = Sprite.Create(Resources.Load<Texture2D>("Textures/Close"), new Rect(0, 0, 25, 25), new Vector2(12.5f, 12.5f));
            m_CloseButton = m_CloseB.AddComponent<Button>();

            m_Content.transform.SetParent(m_gameObject.transform);
            m_CloseB.transform.SetParent(m_gameObject.transform);

            m_CloseButton.onClick.AddListener(onCloseClicked);

            m_ContentRect.pivot = new Vector2(0, 1);
            m_ContentRect.anchorMin = new Vector2(0, 0);
            m_ContentRect.anchorMax = new Vector2(1, 1);
            m_ContentRect.offsetMax = new Vector2(0, 0);
            m_ContentRect.offsetMin = new Vector2(0, 0);
            m_ContentRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, m_TitleHeight, m_Rect.rect.height - m_TitleHeight);

            m_CloseRect.anchorMin = new Vector2(1, 1);
            m_CloseRect.anchorMax = new Vector2(1, 1);
            m_CloseRect.sizeDelta = new Vector2(m_TitleHeight, m_TitleHeight);
            m_CloseRect.pivot = new Vector2(1, 1);

            Drag drag = m_gameObject.AddComponent<Drag>();
        }

        public override GameObject getObject()
        {
            return m_Content;
        }

        protected void onCloseClicked()
        {
            hide();
        }
    }
}

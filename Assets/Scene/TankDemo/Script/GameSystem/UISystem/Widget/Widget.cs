using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WaterBox.Widgets
{
    public class Widget : MonoBehaviour, IPointerClickHandler
    {
        protected enum WidgetType
        {
            TYPE_WIDGET,
            TYPE_DIALOG
        };
        bool IsInit= false;
        protected WidgetType m_Type;
        public GameObject m_gameObject;
        public RectTransform m_Rect { get; set; }
        public Image m_Image { get; set; }

        bool m_IsHidden;

        public Widget()
        {
            m_Type = WidgetType.TYPE_WIDGET;
            m_IsHidden = false;
        }
        private void Start()
        {
            if (m_gameObject != null && !IsInit)
            {
                init(m_gameObject);
            }
        }
        public virtual void init(GameObject obj)
        {
            m_gameObject = obj;
            IsInit = true;
            resetParent();
            m_gameObject.name = "WidgetUI";
            m_Rect=m_gameObject.GetComponent<RectTransform>();
            m_Image = m_gameObject.GetComponent<Image>();

            if (!m_Rect)
                m_Rect = m_gameObject.AddComponent<RectTransform>();
            if (!m_Image)
                m_Image = m_gameObject.AddComponent<Image>();
            m_Image.sprite = Resources.Load<Sprite>("Background");
            

            m_gameObject.SetActive(!m_IsHidden);
        }

        /// <summary>
        /// 重置父节点
        /// </summary>
        public void resetParent()
        {
            if(!m_gameObject.transform.parent)
            {
                GameObject gameobj = GameObject.Find("UI");
                m_gameObject.transform.SetParent(gameobj.transform);
            }
            
        }
        public void update()
        {
        }


        public virtual GameObject getObject()
        {
            return m_gameObject;
        }

        public void setHidden(bool hidden)
        {
            m_IsHidden = hidden;
            if(m_IsHidden)
            {
                hide();
            }
            else
            {
                show();
            }
        }
        public bool isHidden()
        {
            return m_IsHidden;
        }

        public void show()
        {
            m_gameObject.SetActive(true);
        }
        public void hide()
        {
            m_gameObject.SetActive(false);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {

            }
        }
    }
}

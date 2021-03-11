using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WaterBox
{
    public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 m_offset;
        public Action onDragBegin { get; set; }
        public Action onDragEnd { get; set; }
        public Action onDrag { get; set; }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (onDragBegin != null)
            {
                onDragBegin();
            }

            // 存储点击时的鼠标坐标
            Vector3 tWorldPos;
            //UI屏幕坐标转换为世界坐标
            RectTransform m_rt = GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rt, eventData.position, eventData.pressEventCamera, out tWorldPos);
            //计算偏移量   
            m_offset = transform.position - tWorldPos;
            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                onDrag();
            }
            SetDraggedPosition(eventData);
        }

        //结束拖拽触发
        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDragEnd != null)
            {
                onDragEnd();
            }
            //SetDraggedPosition(eventData);
        }
        
        private void SetDraggedPosition(PointerEventData eventData)
        {
            //存储当前鼠标所在位置
            Vector3 globalMousePos;
            //UI屏幕坐标转换为世界坐标
            RectTransform m_tr = GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_tr, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                //设置位置及偏移量
                m_tr.position = globalMousePos + m_offset;
            }
        }

    }
}
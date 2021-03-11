using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WaterBox
{
    class Drop : MonoBehaviour, IDropHandler
    {
        public Action<PointerEventData > onDrop { get; set; }
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
            {
                onDrop(eventData);
            }
        }
    }
}

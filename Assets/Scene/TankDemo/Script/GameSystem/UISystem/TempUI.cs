using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox
{
    class TempUI
    {
        GameObject m_Time;
        public TempUI()
        {
            EventSystem.get().registrationEvent<int>(EventEnum.getEventEnumString(EventEnum.EventEnumType.TIMER_CLICK), onClick);
            m_Time = GameObject.Find("Time");
        }

        void onClick(int time)
        {
            Text text = m_Time.GetComponent<Text>();
            
            text.text = "Date = " + Date.convertTimeToDate(time).toString();
        }
    }
}

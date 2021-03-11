using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class Event
    {
        public Event(String eventName, Callback function)
        {
            m_EventName = eventName;
            m_Function = function;
        }

        public String getEventName()
        {
            return m_EventName;
        }

        public void setParmList(Varibles parmList)
        {
            m_ParmList = parmList;
        }

        public void execute()
        {
            bool isSucceed = m_Function.run(m_ParmList);
            if(!isSucceed)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Assert(false, "Event::execute return false, eventName = " + m_EventName + "Parm Num = " + m_ParmList.getNumVarible());
#endif
            }
        }

        Callback m_Function;
        String m_EventName;
        Varibles m_ParmList;
    }
}

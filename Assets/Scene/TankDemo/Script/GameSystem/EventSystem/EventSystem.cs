using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{ 
    class EventMessage 
    {
        public EventMessage()
        {
            m_CallbackParms = new Varibles();
            m_EventName = null;
        }
        public String m_EventName { set; get; }
        public Varibles m_CallbackParms { set; get; }
    }
    class EventSystem
    {
        static public EventSystem get()
        {
            return m_Instance;
        }
        EventSystem()
        {
            m_EventRunQueue = new List<EventMessage>();
            m_EventList = new List<Event>();
        }
        public void registrationEvent(String eventName, Action func)
        {
            Event event1 = new Event(eventName, Callback.makeCallback(func));
            m_EventList.Add(event1);
        }
        public void registrationEvent<P1>(String eventName, Action<P1> func)
        {
            Event event1 = new Event(eventName, Callback.makeCallback(func));
            m_EventList.Add(event1);
        }
        public void registrationEvent<P1, P2>(String eventName, Action<P1, P2> func)
        {
            Event event2 = new Event(eventName, Callback.makeCallback(func));
            m_EventList.Add(event2);
        }
        public void registrationEvent(EventEnum.EventEnumType eventType, Action func)
        {
            string str = EventEnum.getEventEnumString(eventType);
            registrationEvent(str, func);
        }
        public void registrationEvent<P1>(EventEnum.EventEnumType eventType, Action<P1> func)
        {
            string str = EventEnum.getEventEnumString(eventType);
            registrationEvent(str, func);
        }
        public void registrationEvent<P1, P2>(EventEnum.EventEnumType eventType, Action<P1, P2> func)
        {
            string str = EventEnum.getEventEnumString(eventType);
            registrationEvent(str, func);
        }
        public void emitEvent(String eventName)
        {
            EventMessage eventMessage = new EventMessage();
            Varibles parms = new Varibles();
            eventMessage.m_EventName = eventName;
            eventMessage.m_CallbackParms = parms;
            m_EventRunQueue.Add(eventMessage);
        }
        public void emitEvent<P1>(String eventName, P1 parm1)
        {
            EventMessage eventMessage = new EventMessage();
            Varibles parms = new Varibles();

            parms.addVarible(new Varible<P1>(parm1));
            eventMessage.m_EventName = eventName;
            eventMessage.m_CallbackParms = parms;
            m_EventRunQueue.Add(eventMessage);
        }
        public void emitEvent<P1, P2>(String eventName, P1 parm1, P2 parm2)
        {
            EventMessage eventMessage = new EventMessage();
            Varibles parms = new Varibles();
            parms.addVarible(new Varible<P1>(parm1));
            parms.addVarible(new Varible<P2>(parm2));
            eventMessage.m_EventName = eventName;
            eventMessage.m_CallbackParms = parms;
            m_EventRunQueue.Add(eventMessage);
        }
        public void emitEvent(EventEnum.EventEnumType eventType)
        {
            var str = EventEnum.getEventEnumString(eventType);
            emitEvent(str);
        }
        public void emitEvent<P1>(EventEnum.EventEnumType eventType, P1 parm1)
        {
            var str = EventEnum.getEventEnumString(eventType);
            emitEvent(str, parm1);
        }
        public void emitEvent<P1, P2>(EventEnum.EventEnumType eventType, P1 parm1, P2 parm2)
        {
            var str = EventEnum.getEventEnumString(eventType);
            emitEvent(str, parm1, parm2);
        }
        public void init()
        {
        }

        public void update()
        {
            int eventCount = m_EventList.Count();
            for(int i=0; i< m_EventRunQueue.Count(); ++i)
            {
                for (int j=0; j< eventCount; ++j)
                {
                    String eventName = m_EventList[j].getEventName();
                    String messageName = m_EventRunQueue[i].m_EventName;
                    Varibles parmList = m_EventRunQueue[i].m_CallbackParms;
                    if (messageName == eventName)
                    {
                        m_EventList[j].setParmList(parmList);
                        m_EventList[j].execute();
                        break;
                    }
                }
            }

            m_EventRunQueue.Clear();
        }

        List<Event> m_EventList;                    //  所有注册的事件
        List<EventMessage> m_EventRunQueue;         //  所有等待执行的事件
        static EventSystem m_Instance = new EventSystem();
    }
}

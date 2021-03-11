using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WaterBox
{
    class Timer:GameSystem
    {
        System.Timers.Timer m_Timer;
        int m_Time;
        int m_Year;

        public Timer(GameSystemMG manager):base(manager)
        {
        }
        public override void init()
        {
            m_Time = 0;
            m_Year = 0;
            m_Timer = new System.Timers.Timer(1000);
            m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(timeTick);//到达时间的时候执行事件；
            m_Timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            m_Timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            m_Timer.Start();

        }
        int getTime()
        {
            return m_Time;
        }
        Date getDate()
        {
            Date date = Date.convertTimeToDate(m_Time);
            return date;
        }
        void timeTick(object source, System.Timers.ElapsedEventArgs e)
        {
            if (m_Time >= 518400)
            {
                //  过年了
                m_Time = 0;
                m_Year++;
            }
            m_Time++;
            //EventSystem.get().emitEvent(EventEnum.getEventEnumString(EventEnum.EventEnumType.TIMER_CLICK), m_Time);
        }

        public override void load(XmlNode node)
        {
            XmlNode timeConfig = node.SelectSingleNode("time");
            XmlNode yearConfig = node.SelectSingleNode("year");
            if (timeConfig != null)
            {
                m_Time = int.Parse(timeConfig.InnerText);
            }
            if (yearConfig != null)
            {
                m_Year = int.Parse(yearConfig.InnerText);
            }
        }

        public override void save(XmlDocument document, XmlElement parent)
        {
            XmlElement timeConfig = document.CreateElement("", "time", "");
            timeConfig.InnerText = "" + m_Time;
            parent.AppendChild(timeConfig);

            XmlElement yearConfig = document.CreateElement("", "year", "");
            yearConfig.InnerText = "" + m_Year;
            parent.AppendChild(yearConfig);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace WaterBox
{
    public class Date
    {
        byte m_Month { get; set; }
        byte m_Day { get; set; }
        byte m_Hour { get; set; }
        byte m_Minue { get; set; }
        public Date(int time = 0)
        {
            convertTimeToDate(this, time);
        }
        public string toString()
        {
            return "" + m_Month + "." + m_Day + "." + m_Hour + "" + m_Minue + "";
        }
        static public Date convertTimeToDate(int time)
        {
            Date date = new Date();
            convertTimeToDate(date, time);
            return date;
        }
        static public void convertTimeToDate(Date date, int time)
        {
            int tempValue;
            date.m_Minue = (byte)(time % 60);
            tempValue = time / 60;
            date.m_Hour = (byte)(tempValue % 24);
            tempValue = tempValue / 24;
            date.m_Day = (byte)(tempValue % 30);
            tempValue = tempValue / 30;
            date.m_Month = (byte)tempValue;
        }
    }

    public class UtilsCommon
    {
        public static string enumToString<T>(T enumValue)
        {
            string str = Enum.GetName(typeof(T), enumValue);
            return str;
        }
        public static T stringToEnum<T>(string enumValue)
        {
            T str = (T)Enum.Parse(typeof(T), enumValue);
            return str;
        }

        public static void log(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log(message);
#endif
        }
        public static void warning(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(message);
#endif
        }
        public static void error(object message)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(message);
#endif
        }
    }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WaterBox
{
    public abstract class Callback
    {
        public static Callback makeCallback(Action callback)
        {
            CallbackContent0 content = CallbackContent0.create(callback);
            return content;
        }
        public static Callback makeCallback<A1>(Action<A1> callback)
        {
            CallbackContent1<A1> content = CallbackContent1<A1>.create(callback);
            return content;
        }
        public static Callback makeCallback<A1, A2>(Action<A1, A2> callback)
        {
            CallbackContent2<A1, A2> content = CallbackContent2<A1, A2>.create(callback);
            return content;
        }
        public virtual bool run(Varibles varible)
        {
            int parmNumber = varible.getNumVarible();
            if (parmNumber != m_ParmNumber)
            {
                errorParmLength(parmNumber);
                return false;
            }
            return true;
        }

        public uint getParmNumber()
        {
            return m_ParmNumber;
        }

        private void errorParmLength(int parmNumber)
        {
            Console.WriteLine("Error: Callback run bad parameter number" + parmNumber+". parameter number is " + m_ParmNumber+"of callback");
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(false, "Error: Callback run bad parameter number" + parmNumber + ". parameter number is " + m_ParmNumber + "of callback");
#endif
        }

        protected void errorParmType(string actionName)
        {
            Console.WriteLine("Error: Callback \""+actionName + "\"" + "type error");
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(false, "Error: Callback \"" + actionName + "\"" + "type error");
#endif
        }

        protected void functionRunFaild(Object classObj, Object functionObj)
        {
            string className = "unkow";
            string functionName = "unkow";
            if(classObj != null)
            {
                className = classObj.ToString();
            }
            if(functionObj != null)
            {
                functionName = functionObj.ToString();
            }
            Console.WriteLine("Error: Callback class name = \"" + className + "\"" + " function name = \"" + functionName + "\" run faild");
#if UNITY_EDITOR
            UnityEngine.Debug.Assert(false, "Error: Callback class name = \"" + className + "\"" + " function name = \"" + functionName + "\" run faild");
#endif
        }

        protected uint m_ParmNumber = 0;
        protected Callback m_Instance;
    }

    class CallbackContent0 : Callback
    {
        public static CallbackContent0 create(Action callback)
        {
            CallbackContent0 content = new CallbackContent0();
            content.m_Callback = callback;
            content.m_ParmNumber = 0;
            content.m_Instance = content;
            return content;
        }
        public override bool run(Varibles varible)
        {
            if(base.run(varible))
            {
                try
                {
                    m_Callback();
                    return true;
                }
                catch (NullReferenceException)
                {
                    functionRunFaild(m_Callback.Target, m_Callback.Method);
                    errorParmType(m_Callback.ToString());
                    return false;
                }
            }
            return false;
        }
        private Action m_Callback;
    }

    class CallbackContent1<A1> :Callback
    {
        public static CallbackContent1<A1> create(Action<A1> callback)
        {
            CallbackContent1<A1> content = new CallbackContent1<A1>();
            content.m_Callback = callback;
            content.m_ParmNumber = 1;
            content.m_Instance = content;
            return content;
        }
        public override bool run(Varibles varible)
        {
            if (base.run(varible))
            {
                try
                {
                    m_Callback(varible[0].get<A1>());
                    return true;
                }
                catch (NullReferenceException)
                {
                    functionRunFaild(m_Callback.Target, m_Callback.Method);
                    errorParmType(m_Callback.ToString());
                    return false;
                }
            }
            return false;
        }
        private Action<A1> m_Callback;
    }
    class CallbackContent2<A1,A2>:Callback
    {
        public static CallbackContent2<A1, A2> create(Action<A1, A2> callback)
        {
            CallbackContent2<A1, A2> content = new CallbackContent2<A1, A2>();
            content.m_Callback = callback;
            content.m_ParmNumber = 2;
            content.m_Instance = content;
            return content;
        }

        public override bool run(Varibles varible)
        {
            if (base.run(varible))
            {
                try
                {
                    m_Callback(varible[0].get<A1>(), varible[1].get<A2>());
                    return true;
                }
                catch (NullReferenceException)
                {
                    functionRunFaild(m_Callback.Target, m_Callback.Method);
                    errorParmType(m_Callback.ToString());
                    return false;
                }
            }
            return false;
        }
        private Action<A1,A2> m_Callback;
    }

}

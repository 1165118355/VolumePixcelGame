using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    public class Varible
    {
        public enum VaribleType
        {
            VARIBLE_TYPE_NONE,
            VARIBLE_TYPE_INT,
            VARIBLE_TYPE_FLOAT,
            VARIBLE_TYPE_DOUBLE,
            VARIBLE_TYPE_STRING,
            VARIBLE_TYPE_VEC3,
            VARIBLE_TYPE_IVEC3,
        };


        Varible()
        {
        }

        /// <summary>
        /// 创建一个Int型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Varible create()
        {
            Varible var = new Varible();
            return var;
        }
        public static Varible create(int value)
        {
            Varible var = new Varible();
            var.setInt(value);
            return var;
        }
        public static Varible create(float value)
        {
            Varible var = new Varible();
            var.setFloat(value);
            return var;
        }
        public static Varible create(string value)
        {
            Varible var = new Varible();
            var.setString(value);
            return var;
        }
        public static Varible create(Vector3 value)
        {
            Varible var = new Varible();
            var.setVec3(value);
            return var;
        }
        public static Varible create(Vector3Int value)
        {
            Varible var = new Varible();
            var.setIVec3(value);
            return var;
        }

        public VaribleType getType()
        {
            return m_Type;
        }

        public void setInt(int value)
        {
            m_Type = VaribleType.VARIBLE_TYPE_INT;
            m_ValueInt = value;
        }
        public int getInt()
        {
            return m_ValueInt;
        }

        public void setFloat(float value)
        {
            m_Type = VaribleType.VARIBLE_TYPE_FLOAT;
            m_ValueFloat = value;
        }
        public float getFloat()
        {
            return m_ValueFloat;
        }
        public void setString(string value)
        {
            m_Type = VaribleType.VARIBLE_TYPE_STRING;
            m_ValueString = value;
        }
        public string getString()
        {
            return m_ValueString;
        }

        public void setVec3(Vector3 value)
        {
            m_Type = VaribleType.VARIBLE_TYPE_VEC3;
            m_ValueVec3 = value;
        }
        public Vector3 getVec3()
        {
            return m_ValueVec3;
        }
        public void setIVec3(Vector3Int value)
        {
            m_Type = VaribleType.VARIBLE_TYPE_IVEC3;
            m_ValueIVec3 = value;
        }
        public Vector3Int getIVec3()
        {
            return m_ValueIVec3;
        }

        VaribleType m_Type = VaribleType.VARIBLE_TYPE_NONE;
        int m_ValueInt;
        float m_ValueFloat;
        double m_ValueDouble;
        string m_ValueString;
        Vector3 m_ValueVec3;
        Vector3Int m_ValueIVec3;
    }

    public class VaribleBase
    {
        public RET get<RET>()
        {
            Varible<RET> var = this as Varible<RET>;
            Debug.Assert(var.get() != null, "Varible<RET> == null");
            return var.get();
        }
    }

    class Varible<T> : VaribleBase
    {
        public Varible(T t)
        {
            m_Varible = t;
        }
        public T get()
        {
            return m_Varible;
        }
        T m_Varible;
    }

    public class Varibles
    {
        public Varibles()
        {
            m_Varibles = new List<VaribleBase>();
        }

        public void addVarible(VaribleBase var)
        {
            m_Varibles.Add(var);
        }

        public int getNumVarible()
        {
            return m_Varibles.Count;
        }
        public VaribleBase getVarible(int index)
        {
            return m_Varibles[index];
        }

        public VaribleBase this[int index]
        {
            get { return m_Varibles[index]; }
        }

        List<VaribleBase> m_Varibles;
    }
}

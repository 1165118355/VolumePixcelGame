using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class Creature : Animal
    {
        /// <summary>
        ///     强壮值
        /// </summary>
        float m_StrongValue;
        public float StrongValue
        {
            get { return m_StrongValue; }
            set { m_StrongValue = value; }
        }

        /// <summary>
        ///     最大血量
        /// </summary>
        float m_BloodValueMax;
        float m_BloodValue;
        public float BloodValueMax
        {
            get { return m_BloodValueMax; }
            set { m_BloodValueMax = value; }
        }
        public float BloodValue
        {
            get { return m_BloodValue; }
            set { m_BloodValue = value; }
        }

        /// <summary>
        ///     最大体力
        /// </summary>
        float m_StaminaValueMax;
        float m_StaminaValue;
        public float StaminaValueMax
        {
            get { return m_StaminaValueMax; }
            set { m_StaminaValueMax = value; }
        }
        public float StaminaValue
        {
            get { return m_StaminaValue; }
            set { m_StaminaValue = value; }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class Animal
    {
        protected Animal()
        {
            init();
        }
        protected virtual void init()
        {
            if(m_BodyPartsManager != null)
            {
                m_BodyPartsManager.init();
            }
            if (m_EquipManager != null)
            {
                m_EquipManager.init();
            }
        }
        protected AnimalBodyParts m_BodyPartsManager;
        protected AnimalEquipManager m_EquipManager;
    }
}
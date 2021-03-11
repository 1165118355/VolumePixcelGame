using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class Human : Creature
    {
        Human()
        {
        }
        protected override void init()
        {
            m_BodyPartsManager = new HumanBodyParts();
            m_EquipManager = new HumanEquipManager();
            base.init();
        }
    }
}
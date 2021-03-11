using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class AnimalBodyParts
    {
        public virtual void init()
        { 
        }
        public int getNumBodyParts()
        {
            return m_BodyParts.Count;
        }
        public void addBodyParts(BodyPart bodyPart)
        {
            m_BodyParts.Add(bodyPart);
        }
        public BodyPart getBodyPart(int index)
        {
            return m_BodyParts[index];
        }

        List<BodyPart> m_BodyParts;
    }
}

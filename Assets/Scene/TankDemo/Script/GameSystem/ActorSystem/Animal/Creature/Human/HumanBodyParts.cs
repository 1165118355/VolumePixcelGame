using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class HumanBodyParts : AnimalBodyParts
    {
        public enum BodyPartType
        {
            PART_HEAD,
            PART_BOYD,
            PART_HAND_LEFT,
            PART_HAND_RIGHT,
            PART_FOOT_LEFT,
            PART_FOOT_RIGHT,
        }

        public override void init()
        {
            foreach (BodyPartType e in Enum.GetValues(typeof(BodyPartType)))
            {
                BodyPart bodyPart = new BodyPart();
                bodyPart.Name = UtilsCommon.enumToString(e);
                addBodyParts(bodyPart);
            }
        }
    }
}

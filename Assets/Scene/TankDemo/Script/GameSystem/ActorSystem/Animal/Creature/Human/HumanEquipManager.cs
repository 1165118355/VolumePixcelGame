using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class HumanEquipManager : AnimalEquipManager
    {
        public enum EquipType
        {
            EQUIP_HEADPIECE,    //  头盔哟
            EQUIP_CLOTHES,      //  衣服哟
            EQUIP_TROUSERS,     //  裤子哟
            EQUIP_SHOE,         //  鞋子哟
        }
        public override void init()
        {
            foreach (EquipType e in Enum.GetValues(typeof(EquipType)))
            {
            }
        }
    }
}

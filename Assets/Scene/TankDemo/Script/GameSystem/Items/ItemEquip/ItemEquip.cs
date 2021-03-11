using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    class ItemEquip : Item
    {
        public ItemEquip()
        {
            m_IconPath = "Textures/Items/Equip/";
            m_Type = Item.ItemType.ITEM_EQUIP;
            m_Size = new Vector2Int(9, 9);
        }
    }
}

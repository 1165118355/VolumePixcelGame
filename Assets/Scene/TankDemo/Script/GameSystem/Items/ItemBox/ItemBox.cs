using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{ 
    public class ItemBox:Item
    {
        public ItemBox()
        {
            m_IconPath = "Textures/Items/Box/";
            m_Type = Item.ItemType.ITEM_BOX;
            m_Size = new Vector2Int(2, 2);
        }
    }
}

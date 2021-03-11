using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    class ItemMaterial:Item
    {
        protected ItemMaterial()
        {
            m_IconPath = "Textures/Items/Material/";
            m_Type = Item.ItemType.ITEM_BOX;
            m_Size = new Vector2Int(1, 1);
        }
    }
}

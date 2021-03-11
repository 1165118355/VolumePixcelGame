using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    public class Stone:ItemBox
    {
        public Stone()
        {
            m_ID = ItemEnum.ItemsID.ITEM_BOX_STONE;
            m_Name = "石头块";
            m_Describe = "一块大石头";
            m_Icon = m_IconPath + "Stone";
            m_PileMax = 1;
            m_PileItemID = ItemEnum.ItemsID.ITEM_BOX_STONE;
        }
    }
}

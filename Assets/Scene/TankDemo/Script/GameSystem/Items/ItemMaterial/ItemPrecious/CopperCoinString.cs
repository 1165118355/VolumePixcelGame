using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class CopperCoinString:ItemMaterial
    {
        public CopperCoinString()
        {
            m_ID = ItemEnum.ItemsID.ITEM_MATERIAL_COPPER_STRING;
            m_Name = "铜钱串";
            m_Describe = "我把一堆铜钱串起来了，这样带着真方便。";
            m_Icon = m_IconPath + "CopperString";
            m_PileMax = 1;
        }
    }
}

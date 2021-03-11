using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class CopperCoin : ItemMaterial
    {
        public CopperCoin()
        {
            m_ID = ItemEnum.ItemsID.ITEM_MATERIAL_COPPER_COIN;
            m_Name = "铜钱";
            m_Describe = "一枚圆形的铜币，铜币中间有个正方形的小孔，似乎可以把他们串起来";
            m_Icon = m_IconPath + "CopperCoin";
            m_PileMax = 20;
            m_PileItemID = ItemEnum.ItemsID.ITEM_MATERIAL_COPPER_COIN;
        }
    }
}

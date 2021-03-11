using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class GrassLine : ItemMaterial
    {
        public GrassLine()
        {
            m_ID = ItemEnum.ItemsID.ITEM_MATERIAL_GRASS_LINE;
            m_Name = "草线";
            m_Describe = "用草纤维简单的搓成一条细线，不是很结实。";
            m_Icon = m_IconPath + "GrassLine";
        }
    }
}

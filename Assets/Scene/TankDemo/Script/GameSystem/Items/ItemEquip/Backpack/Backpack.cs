using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    class Backpack: ItemEquip
    {
        public Vector2Int m_Size;
        public Widgets.BackpackDialog m_BackpackUI;
        public Backpack()
        {
            m_ID = ItemEnum.ItemsID.ITEM_EQUIP_BACKPACK;
            m_Name = "背包";
            m_Describe = "又大又丑的背包，但却是长途跋涉者必备物品之一。";
            m_Icon = m_IconPath + "Backpack";
            m_PileMax = 1;
            m_PileItemID = ItemEnum.ItemsID.ITEM_EQUIP_BACKPACK;
            m_BackpackUI = new Widgets.BackpackDialog();

        }

        /// 使用
        public virtual int employ()
        {
            m_BackpackUI.show();
            return 1;
        }
    }
}

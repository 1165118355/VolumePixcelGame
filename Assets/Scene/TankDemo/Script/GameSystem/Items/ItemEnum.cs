using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    public class ItemEnum
    {
        public enum ItemsID
        {
            //  Box
            ITEM_BOX_BEGIN,
            ITEM_BOX_SOIL,
            ITEM_BOX_STONE,
            ITEM_BOX_LIMESTONE,
            ITEM_BOX_GRIOTTE,
            ITEM_BOX_END,

            //  Material
            ITEM_MATERIAL_BEGIN,
            ITEM_MATERIAL_UNKOW,
            ITEM_MATERIAL_W,
            ITEM_MATERIAL_A,
            ITEM_MATERIAL_T,
            ITEM_MATERIAL_E,
            ITEM_MATERIAL_R,
            ITEM_MATERIAL_B,
            ITEM_MATERIAL_O,
            ITEM_MATERIAL_X,
            ITEM_MATERIAL_COPPER_COIN,
            ITEM_MATERIAL_COPPER_STRING,
            ITEM_MATERIAL_GRASS_LINE,
            ITEM_MATERIAL_END,

            //  Equip
            ITEM_EQUIP_BACKPACK,
        }
        public static String getItemEnumString(ItemEnum.ItemsID enumValue)
        {
            String str = Enum.GetName(typeof(ItemEnum.ItemsID), enumValue);
            return str;
        }
    }
}

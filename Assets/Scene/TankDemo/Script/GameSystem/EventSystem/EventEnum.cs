using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class EventEnum
    {
        public enum EventEnumType
        {
            EXIT,
            TERRAIN_BLOCK_REMOVE,
            TERRAIN_BLOCK_ADD,
            TERRAIN_SAVE,

            GLOBALCONFIG_SAVE,

            BACKPACK_ITEM_ADD,

            ITEM_CREATE,

            UI_CONSOLE_STATE_CHANGED_int,

            TIMER_CLICK
        }
        public static String getEventEnumString(EventEnumType enumValue)
        {
            String str = Enum.GetName(typeof(EventEnumType), enumValue);
            return str;
        }
    }
}

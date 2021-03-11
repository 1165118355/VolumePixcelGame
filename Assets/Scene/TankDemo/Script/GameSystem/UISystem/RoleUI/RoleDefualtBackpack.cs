using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox.Widgets
{
    public class RoleDefualtBackpack : BackpackUI
    {
        public override void init(GameObject obj)
        {
            base.init(obj);

            string eventNameAdd = EventEnum.getEventEnumString(EventEnum.EventEnumType.BACKPACK_ITEM_ADD);
            EventSystem.get().registrationEvent<int>(eventNameAdd, addItem);
        }
    }
}
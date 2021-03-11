using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class ItemSystem :GameSystem
    {
        ItemCreator m_ItemCreator;
        public ItemSystem(GameSystemMG manager):base(manager)
        {
        }

        public override void init()
        {
            ItemCreator.Instance.init();
        }
        public override void update()
        {
        }

        public override void shutdown()
        {
        }
    }
}

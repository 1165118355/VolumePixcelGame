using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class ItemCombiner
    {
        static ItemCombiner m_Instance = new ItemCombiner();
        static public ItemCombiner Instance
        {
            get { return m_Instance; }
        }

        Item itemCombination(Item item1, Item item2)
        {
            return new Item();
        }

    }
}

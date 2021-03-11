using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace WaterBox
{
    abstract class ItemObject
    {
        public abstract Item create();
    }
    class ItemObjectType<T>: ItemObject
            where T : new()
    {
        public override Item create()
        {
            T t = new T();
            if(t is Item)
            {
                return t as Item;
            }
            else
            {
                return null;
            }
        }
    }
    class ItemCreator
    {
        Dictionary<string, ItemObject> m_ItemObjectType;
        static ItemCreator m_Instance = new ItemCreator();
        static public ItemCreator Instance
        {
            get { return m_Instance; }
        }
        ItemCreator()
        {
            m_ItemObjectType = new Dictionary<string, ItemObject>();
        }
        public void init()
        {
            itemRegistration(ItemEnum.getItemEnumString(ItemEnum.ItemsID.ITEM_BOX_STONE), new ItemObjectType<Stone>());

            itemRegistration(ItemEnum.getItemEnumString(ItemEnum.ItemsID.ITEM_MATERIAL_COPPER_COIN), new ItemObjectType<CopperCoin>());
            itemRegistration(ItemEnum.getItemEnumString(ItemEnum.ItemsID.ITEM_MATERIAL_COPPER_STRING), new ItemObjectType<CopperCoinString>());
            itemRegistration(ItemEnum.getItemEnumString(ItemEnum.ItemsID.ITEM_MATERIAL_GRASS_LINE), new ItemObjectType<GrassLine>());

            itemRegistration(ItemEnum.getItemEnumString(ItemEnum.ItemsID.ITEM_EQUIP_BACKPACK), new ItemObjectType<Backpack>());
        }

        /// <summary>
        /// 注册Item
        /// </summary>
        /// <param name="itemTypeName"></param>
        /// <param name="obj"></param>
        public void itemRegistration(string itemTypeName, ItemObject obj)
        {
            if(!m_ItemObjectType.ContainsKey(itemTypeName))
            {
                m_ItemObjectType.Add(itemTypeName, obj);
            }
        }

        /// <summary>
        /// 创建Itme
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public Item createItem(ItemEnum.ItemsID itemID)
        {
            string itemTypeName = ItemEnum.getItemEnumString(itemID);
            if(itemTypeName != null && m_ItemObjectType.ContainsKey(itemTypeName))
            {
                ItemObject obj = m_ItemObjectType[itemTypeName];
                Item item = obj.create();
                if (item != null)
                {
                    return item;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }


        /// 加载
        public Item load(string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlNode xmlnode = xmldoc.SelectSingleNode("item");

            if(xmlnode != null)
            {
                Item item = new WaterBox.Item();
                item.load(xmlnode);
                return item;

            }
            return null;
        }

        /// 保存
        public bool save(Item item, string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmldoc.AppendChild(xmldec);
            XmlElement itemXml = xmldoc.CreateElement("item");
            xmldoc.AppendChild(itemXml);
            return item.save(itemXml);
        }
    }
}

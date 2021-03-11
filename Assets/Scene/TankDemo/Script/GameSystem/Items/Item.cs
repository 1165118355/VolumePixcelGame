using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace WaterBox
{
    public class Item : System.Object
    {
        const string nameItem       = "item";
        const string argType        = "type";
        const string nameTag        = "tag";
        const string nameKey        = "key";
        const string nameValue      = "value";
        const string tagIcon        = "icon";
        const string tagId          = "id";
        const string tagName        = "name";
        const string tagDescribe    = "describe";
        const string tagPlieMax     = "plieMax";
        const string tagPlie        = "plie";
        const string tagSize        = "size";
        const string tagpos         = "pos";
        const string tagIconPath    = "icon_path";

        BlueMap m_ComboMap;
        public enum ItemType
        {
            ITEM_BOX = 1,
            ITEM_MATERIAL = 2,
            ITEM_CONSUMABLE = 4,
            ITEM_EQUIP = 8,
        };
        public Item()
        {
            m_Name = "我不该出现在这里";
            m_Describe = "我是谁，我在哪里，我在做什么！！！";
            m_PileMax = 999;
            m_PileItemID = 0;
            m_Pile = 1;
            m_ID = ItemEnum.ItemsID.ITEM_MATERIAL_UNKOW;
            m_Size = new Vector2Int(1, 1);
            string iconPath = "Textures/Items/Material/";
            m_Icon = iconPath + "Unknow";
        }
        /// 拷贝构造
        public Item(Item copyItem)
        {
            m_Icon = copyItem.m_Icon;
            m_ID = copyItem.m_ID;
            m_Type = copyItem.m_Type;
            m_Name = copyItem.m_Name;
            m_Describe = copyItem.m_Describe;
            m_PileMax = copyItem.m_PileMax;
            m_Pile = copyItem.m_Pile;
            m_PileItemID = copyItem.m_PileItemID;
            m_Size = copyItem.m_Size;
            m_Pos = copyItem.m_Pos;
            m_IconPath = copyItem.m_IconPath;
        }

        /// 
        public void init()
        {
        }

        public virtual void initRegistration()
        {
        }

        /// 使用
        public virtual int employ()
        {
            return 0;
        }

        /// 加载
        public virtual bool load(XmlNode itemNode)
        {
            if (itemNode != null)
            {
                string typeStr = itemNode.Attributes[argType].Value;
                m_Type = UtilsCommon.stringToEnum<ItemType>(typeStr);
                return true;
            }
            return false;
        }

        /// 保存
        public virtual bool save(XmlNode itemNode)
        {
            if (itemNode != null)
            {
                itemNode.Attributes[argType].Value = UtilsCommon.enumToString(m_Type);

                //  保存Icon
                XmlElement icon = itemNode.OwnerDocument.CreateElement(nameTag);
                icon.Attributes[nameKey].Value = tagIcon;
                icon.Attributes[nameValue].Value = m_Icon;
                itemNode.AppendChild(icon);

                //  保存ID
                XmlElement id = itemNode.OwnerDocument.CreateElement(nameTag);
                id.Attributes[nameKey].Value = tagId;
                id.Attributes[nameValue].Value = UtilsCommon.enumToString(m_ID);
                itemNode.AppendChild(id);

                //  保存Name
                XmlElement name = itemNode.OwnerDocument.CreateElement(tagName);
                name.Attributes[nameKey].Value = tagName;
                name.Attributes[nameValue].Value = m_Name;
                itemNode.AppendChild(name);

                //  保存Describe
                XmlElement describe = itemNode.OwnerDocument.CreateElement(tagName);
                describe.Attributes[nameKey].Value = tagDescribe;
                describe.Attributes[nameValue].Value = m_Describe;
                itemNode.AppendChild(describe);

                //  保存PileMax
                XmlElement pileMax = itemNode.OwnerDocument.CreateElement(tagName);
                pileMax.Attributes[nameKey].Value = tagPlieMax; 
                pileMax.Attributes[nameValue].Value = m_PileMax.ToString();
                itemNode.AppendChild(pileMax);

                //  保存Pile
                XmlElement pile = itemNode.OwnerDocument.CreateElement(tagName);
                pile.Attributes[nameKey].Value = tagPlie;
                pile.Attributes[nameValue].Value = m_Pile.ToString();
                itemNode.AppendChild(pile);

                //  保存Size
                XmlElement size = itemNode.OwnerDocument.CreateElement(tagName);
                size.Attributes[nameKey].Value = tagSize;
                size.Attributes[nameValue].Value = m_Size.ToString();
                itemNode.AppendChild(size);

                //  保存Pos
                XmlElement pos = itemNode.OwnerDocument.CreateElement(tagName);
                pos.Attributes[nameKey].Value = tagpos;
                pos.Attributes[nameValue].Value = m_Pos.ToString();
                itemNode.AppendChild(pos);

                //  保存IconPath
                XmlElement iconPath = itemNode.OwnerDocument.CreateElement(tagName);
                iconPath.Attributes[nameKey].Value = tagIconPath;
                iconPath.Attributes[nameValue].Value = m_IconPath;
                itemNode.AppendChild(iconPath);

                return true;
            }
            return false;
        }

        /// 物品的图标路径
        public string m_Icon { get; set; }

        /// 物品ID
        public ItemEnum.ItemsID m_ID { get; set; }

        /// 物品类型
        public ItemType m_Type;

        /// 物品的名字
        public String m_Name { get; set; }

        /// 物品的描述
        public String m_Describe { get; set; }

        /// 物品最大堆叠数
        public int m_PileMax { get; set; }

        /// 堆叠的数量
        public int m_Pile { get; set; }   
          
        /// 可以堆叠的物品ID
        //public ItemCreator.ItemsID m_PileItemID { get; set; }
        public ItemEnum.ItemsID m_PileItemID { get; set; }

        /// 物品的大小(旋转可能)
        public Vector2Int m_Size { get; set; }

        /// 存放与背包的位置
        public Vector2Int m_Pos { get; set; }


        protected string m_IconPath = "Textures/Items/Material/";


        /// <summary>
        ///    获取类型
        /// </summary>
        /// <returns></returns>
        public ItemType getType()
        {
            return m_Type;
        }
    }
}

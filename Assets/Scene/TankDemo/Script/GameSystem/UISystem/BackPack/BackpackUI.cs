using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WaterBox.Widgets
{
    public class BackpackUI :Widget
    {
        public GameObject m_GridContentObj { get; set; }
        Widget m_GridContent;
        GridLayoutGroup m_Layout;
        public Vector2Int m_BackpackSize = new Vector2Int(0, 0);
        BackpackGrid [,] m_Grids;
        Drop m_Drop;

        List<ItemUI> m_Items;

        int m_GridSize = 25;
        int m_Spacing = 0;

        public static GameObject createBackpack(Vector2Int size=new Vector2Int())
        {
            GameObject obj = new GameObject();
            BackpackUI backpack = obj.AddComponent<BackpackUI>();
            backpack.m_BackpackSize = size;
            backpack.init(obj);
            return obj;
        }

        protected BackpackUI()
        {
        }
        public override void init(GameObject obj)
        {
            base.init(obj);
            m_Items = new List<ItemUI>();
            m_GridContentObj = WidgetFactory.Instance.createWidget<Widget>();
            m_GridContent = m_GridContentObj.GetComponent<Widget>();
            m_GridContentObj.transform.SetParent(obj.transform);


            //m_Rect.localPosition = new Vector2(0, 0);
            m_Rect.pivot = new Vector2(0, 1);

            m_GridContent.m_Rect.anchorMin = new Vector2(0, 0);
            m_GridContent.m_Rect.anchorMax = new Vector2(1, 1);
            m_GridContent.m_Rect.offsetMin = new Vector2(0, 0);
            m_GridContent.m_Rect.offsetMax = new Vector2(1, 1);
            m_GridContent.m_Rect.pivot = new Vector2(0, 1);
            m_GridContent.m_Rect.localScale = new Vector3(1,1, 1);


            if (m_BackpackSize.x == 0 ||
                m_BackpackSize.y == 0)
            {
                
                m_BackpackSize.x = (int)m_Rect.rect.width / m_GridSize;
                m_BackpackSize.y = (int)m_Rect.rect.height / m_GridSize;
            }

            m_Grids = new BackpackGrid[m_BackpackSize.x, m_BackpackSize.y];
            m_Layout = m_GridContentObj.AddComponent<GridLayoutGroup>();
            m_Drop = m_GridContentObj.AddComponent<Drop>();
            m_Layout.cellSize = new Vector2(m_GridSize, m_GridSize);
            updateGrid();

            m_Drop.onDrop  = this.onDrop;

        }

        public void updateGrid()
        {
            clear();
            for (int i=0; i<m_BackpackSize.y; ++i)
            {
                for(int j=0; j<m_BackpackSize.x; ++j)
                {
                    GameObject obj = BackpackGrid.createBackpackGrid(m_GridContentObj);
                    m_Grids[j, i] = obj.GetComponent<BackpackGrid>();
                    obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
        }
        public void addItem(int id)
        {
            ItemEnum.ItemsID itemID = (ItemEnum.ItemsID)id;

            Item item = ItemCreator.Instance.createItem(itemID);
            if(item != null)
            {
                addItem(item);
            }
            else
            {
            }
        }


        Vector2Int findVacancy(Item item)
        {
            Vector2Int size = m_BackpackSize;

            Vector2Int itemSize = item.m_Size;
            for (int i = 0; i <= size.y - itemSize.y; ++i)
            {
                for (int j = 0; j <= size.x - itemSize.x; ++j)
                {
                    bool isSuitable = checkPlaceItem(item, j, i);
                    if (isSuitable)
                    {

                        return new Vector2Int(j, i);
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }

        void addItem(Item item)
        {
            //  查找背包中是否有可以堆叠的物品
            //  如果有堆叠的物品则堆叠
            Item plieItem = null;
            for(int i=0; i<m_Items.Count; ++i)
            {
                Item itemBackpack = m_Items[i].Item;
                if(itemBackpack.m_Pile + item.m_Pile < itemBackpack.m_PileMax && 
                    itemBackpack.m_ID == item.m_PileItemID)
                {
                    plieItem = itemBackpack;
                    plieItem.m_Pile += item.m_Pile;
                    m_Items[i].updateText();
                    break;
                }
            }

            //否则放在空位
            if (plieItem == null)
            { 
                Vector2Int pos = findVacancy(item);
                if (pos.x != -1 && pos.y != -1)
                {
                    GameObject itemUIObj = ItemUI.createItemUI(this, item);
                    ItemUI itmeUI = itemUIObj.GetComponent<ItemUI>();
                    placeItem(itmeUI, pos.x, pos.y);
                }
                else
                {
                    UtilsCommon.log("你的背包装不下该物品：" + item.m_Name+", 自动丢弃");
                }
            }
        }

        /// <summary>
        /// 拿走一个道具
        /// </summary>
        /// <param name="item"></param>
        public void takeItem(Item item)
        {
            Vector2Int itemPos = item.m_Pos;
            Vector2Int itemSize = item.m_Size;
            for (int ii = itemPos.y; ii < itemPos.y + itemSize.y; ++ii)
            {
                for (int jj = itemPos.x; jj < itemPos.x + itemSize.x; ++jj)
                {
                    m_Grids[jj, ii].m_Item = null;
                    m_Grids[jj, ii].m_Image.color = new Color(1, 1, 1, 128);
                }
            }

            for (int i=0; i<m_Items.Count; ++i)
            {
                if (m_Items[i].Item == item)
                {
                    m_Items[i].resetParent();
                    m_Items.RemoveAt(i);
                    break;
                }
            }
        }

        bool checkPlaceItem(Item item, int x, int y)
        {
            Vector2Int itemSize = item.m_Size;
            for (int ii = y; ii < y + itemSize.y; ++ii)
            {
                for (int jj = x; jj < x + itemSize.x; ++jj)
                {
                    BackpackGrid agrid = m_Grids[jj, ii];
                    if (agrid.m_Item != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        Item getItem(int x, int y)
        {
            if(m_BackpackSize.x > x && m_BackpackSize.y > y)
            {
                BackpackGrid agrid = m_Grids[x, y];
                if (agrid.m_Item != null)
                {
                    return agrid.m_Item;
                }
            }
            return null;
        }
        public bool placeItem(ItemUI itemUI)
        {
            Vector2Int findPos = findVacancy(itemUI.Item);
            if (findPos.x != -1 && findPos.y != -1)
            {
                placeItem(itemUI, findPos.x, findPos.y);
            }
            else
            {
                return false;
            }
            return true;
        }

        public void placeItem(ItemUI itemUI, int x, int y)
        {
            itemUI.transform.SetParent(m_gameObject.transform);
            itemUI.m_Rect.localPosition = new Vector2(x*(m_GridSize+m_Spacing), -y * (m_GridSize + m_Spacing));
            itemUI.setBackpack(this);
            Item item = itemUI.Item;
            item.m_Pos = new Vector2Int(x, y);
            Vector2Int itemSize = item.m_Size;
            for (int ii = y; ii < y + itemSize.y; ++ii)
            {
                for (int jj = x; jj < x + itemSize.x; ++jj)
                {
                    BackpackGrid agrid = m_Grids[jj, ii];
                    agrid.m_Item = item;
                    m_Grids[jj, ii].m_Image.color = new Color(0, 0, 0, 200);
                }
            }

            bool hasItem = false;
            for (int i=0; i<m_Items.Count; ++i)
            {
                if(m_Items[i] == itemUI)
                {
                    hasItem = true;
                }
            }
            if(!hasItem)
            {
                m_Items.Add(itemUI);
            }
        }

        void clear()
        {
        }

        void onDrop(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerDrag;
            ItemUI itemUI= obj.GetComponent<ItemUI>();
            if(itemUI == null)
            {
                return;
            }

            GameObject player = GameObject.Find("MainCamera");
            Camera camera = player.GetComponent<Camera>();

            
            Vector2 pos = new Vector3(eventData.position.x, eventData.position.y, 0) - m_GridContent.m_Rect.position;
            int x =  (int)pos.x  / (m_GridSize + m_Spacing);
            int y = -(int)pos.y  / (m_GridSize + m_Spacing);

            if (x >= 0 && x < m_BackpackSize.x && 
                y >= 0 && y < m_BackpackSize.y)
            {
                // 如果这个位置可以放置物品则放在这里，否则自动添加到背包空位，如果没有空位则销毁
                if(checkPlaceItem(itemUI.Item, x, y))
                {
                    placeItem(itemUI, x, y);
                }
                else
                {
                    Vector2Int findPos = findVacancy(itemUI.Item);
                    if(findPos.x != -1 && findPos.y != -1)
                    {
                        placeItem(itemUI, findPos.x, findPos.y);
                    }
                    else
                    {
                        GameObject.Destroy(obj);
                    }
                }
            }
            else
            {
                GameObject.Destroy(obj);
            }
        }
    }
}

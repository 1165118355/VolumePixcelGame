using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WaterBox.Widgets
{
    public class ItemUI : Widget, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        bool showPanel = false;

        BackpackUI m_Backpck;

        GameObject m_TextObject;
        RectTransform m_TextRect;
        Text m_Text;

        Drag m_Drag;
        CanvasGroup m_CanvasGroup;
        Item m_Item;
        public Item Item
        {
            get { return m_Item; }
            set { m_Item = value; }
        }
        private Vector3 m_offset;
        public int m_PileNumber=0;

        public static GameObject createItemUI(BackpackUI parent, Item item)
        {
            GameObject obj = new GameObject();
            ItemUI itemUI = obj.AddComponent<ItemUI>();
            itemUI.init(obj, parent, item);
            return obj;
        }
        public static GameObject createItemUI(ItemUI copy)
        {
            GameObject obj = new GameObject();
            ItemUI itemUI = obj.AddComponent<ItemUI>();
            Item item = new Item(copy.m_Item);
            itemUI.init(obj, copy.m_Backpck, item);
            return obj;
        }

        ItemUI()
        {
        }
        ItemUI(ItemUI item)
        {
            Item = new Item(item.Item);
        }
        void init(GameObject obj, BackpackUI parent, Item item)
        {
            base.init(obj);

            m_TextObject = new GameObject();
            m_CanvasGroup = m_gameObject.AddComponent<CanvasGroup>();
            m_TextObject.transform.SetParent(m_gameObject.transform);
            m_TextRect = m_TextObject.AddComponent<RectTransform>();
            m_Text = m_TextObject.AddComponent<Text>();

            m_Backpck = parent;
            Item = item;
            m_gameObject.name = "ItemUI";
            m_gameObject.transform.SetParent(parent.m_gameObject.transform);

            m_Rect.anchorMin = new Vector2(0, 1);
            m_Rect.anchorMax = new Vector2(0, 1);
            m_Rect.pivot = new Vector2(0, 1);
            m_Rect.sizeDelta = new Vector2Int(25 * Item.m_Size.x, 25 * Item.m_Size.y);
            m_Rect.localPosition = new Vector2(Item.m_Pos.x * 25, -(Item.m_Pos.y * 25));
            m_Rect.localScale = new Vector3(1,1,1);

            Texture2D image = Resources.Load<Texture2D>(Item.m_Icon);
            if (image != null)
            {
                m_Image.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(image.width / 2, image.height / 2));
            }

            m_Drag = m_gameObject.AddComponent<Drag>();
            m_Drag.onDragBegin = onDragBegin;
            m_Drag.onDragEnd = onDragEnd;
            m_Drag.onDrag = onDrag;

            m_Text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            m_Text.color = new Color(0, 0, 0, 1);

            m_TextRect.sizeDelta = m_Rect.sizeDelta;
            m_TextRect.anchorMin = new Vector2(0, 1);
            m_TextRect.anchorMax = new Vector2(0, 1);
            m_TextRect.pivot = new Vector2(0, 1);
            m_TextRect.localPosition = new Vector2(0, 0);
        }

        float showPanelTime = 0;
        public void Update()
        {
            if (showPanel)
            {
                showPanelTime += Time.deltaTime;
                if (showPanelTime > 0.5)
                {
                    onShowPanel();
                }
            }
            else
            {
                showPanelTime = 0;
            }
        }

        public void updateText()
        {
            if (m_Item != null)
                m_Text.text = "" + Item.m_Pile;
        }

        void onDragBegin()
        {
            var beginPos = m_gameObject.transform.position;
            m_gameObject.transform.SetParent(null);
            m_gameObject.transform.position = beginPos;
            m_Backpck.takeItem(Item);
            m_CanvasGroup.blocksRaycasts = false;
            if (Input.GetKey(KeyCode.LeftControl) &&
                (int) m_Item.m_Pile / 2 > 0)
            {
                GameObject obj = ItemUI.createItemUI(this);
                ItemUI itemUI = obj.GetComponent<ItemUI>();
                itemUI.m_Item.m_Pile /= 2;
                m_Item.m_Pile -= itemUI.m_Item.m_Pile;
                m_Backpck.placeItem(itemUI, itemUI.m_Item.m_Pos.x, itemUI.m_Item.m_Pos.y);
                updateText();
            }
        }
        void onDragEnd()
        {
            m_CanvasGroup.blocksRaycasts = true;
        }
          
        void onDrag()
        {
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            showPanel = true;
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            onHidePanel();
            showPanel = false;
        }
        void onShowPanel()
        {
            GameObject parent = GameObject.Find("UI/ItemDescribeParent");
            GameObject name = GameObject.Find("UI/ItemDescribeParent/ItemDescribe/Name");
            GameObject describe = GameObject.Find("UI/ItemDescribeParent/ItemDescribe/Describe");

            name.GetComponent<Text>().text = m_Item.m_Name;
            describe.GetComponent<Text>().text = m_Item.m_Describe;
            parent.GetComponent<RectTransform>().position = Input.mousePosition;
            parent.transform.SetSiblingIndex(parent.transform.parent.childCount - 1);
        }
        void onHidePanel()
        {
            GameObject parent = GameObject.Find("UI/ItemDescribeParent");
            parent.GetComponent<RectTransform>().position = new Vector2(-100, -100);
            //parent.SetActive(false);
        }

        public void setBackpack(BackpackUI backpack)
        {
            m_Backpck = backpack;
        }


        public void OnDrop(PointerEventData eventData)
        {
            GameObject obj = eventData.pointerDrag;
            ItemUI itemUI = obj.GetComponent<ItemUI>();
            if (itemUI == null)
            {
                return;
            }
            if (m_Item != null &&
                m_Item.m_ID == itemUI.Item.m_PileItemID)
            {
                m_Item.m_Pile += itemUI.Item.m_Pile;
                int surplus = m_Item.m_Pile - m_Item.m_PileMax;
                itemUI.Item.m_Pile = surplus;
                updateText();
                if (surplus <= 0)
                {
                    GameObject.Destroy(obj);
                    return;
                }
                if(m_Backpck.placeItem(itemUI))
                {
                    itemUI.updateText();
                }
            }
            else
            {
                m_Backpck.placeItem(itemUI);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {

            }
        }
    }
}

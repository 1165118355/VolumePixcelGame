using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox.Widgets
{
    
    class BackpackGrid:Widget
    {
        public Item m_Item;

        public static GameObject createBackpackGrid()
        {
            GameObject obj = new GameObject();
            BackpackGrid grad = obj.AddComponent<BackpackGrid>();
            grad.init(obj);
            return obj;
        }
        public static GameObject createBackpackGrid(GameObject parent)
        {
            GameObject obj = BackpackGrid.createBackpackGrid();
            obj.transform.SetParent(parent.transform);
            return obj;
        }
        public BackpackGrid()
        {
        }
        public new void init(GameObject obj)
        {
            base.init(obj);
            Texture2D image = Resources.Load<Texture2D>("Textures/BackpackGrid");
            if (image)
            {
                m_Image.sprite = Sprite.Create(image, new Rect(0, 0, 64, 64), new Vector2(32,32));
            }
        }

        new public GameObject getObject()
        {
            return m_gameObject;
        }
        public Item getItem() {
            return m_Item;
        }

    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WaterBox.Widgets;

namespace WaterBox
{
    public class UISystem : GameSystem
    {

        public UISystem(GameSystemMG gameSystemMG) : base(gameSystemMG)
        {
            registerUI();
        }

        public override void init()
        {
        }

        public override void update()
        {
        }

        public override void shutdown()
        {
        }

        protected void registerUI()
        {
            WidgetFactory.Instance.registerWidget<Widget>();
            WidgetFactory.Instance.registerWidget<Dialog>();
            WidgetFactory.Instance.registerWidget<BackpackDialog>();
            WidgetFactory.Instance.registerWidget<Actions>();
            WidgetFactory.Instance.registerWidget<Action>();
        }
        SceneUIManager m_SceneUIManager;
    }
}

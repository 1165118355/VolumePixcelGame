using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox
{
    public class MainMenu : InterfaceWidget
    {
        public Button Save;
        public Button Exit;
        void Start()
        {
            gameObject.SetActive(false);
            Save.onClick.AddListener(onSaveButtonClicked);
            Exit.onClick.AddListener(onExitButtonClicked);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void onSaveButtonClicked()
        { 
            EventSystem.get().emitEvent(EventEnum.EventEnumType.TERRAIN_SAVE);
            EventSystem.get().emitEvent(EventEnum.EventEnumType.GLOBALCONFIG_SAVE);
        }
        void onExitButtonClicked()
        {
            GameSystemMG.Instance.shutdown();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox
{
    public class ConsoleBox : InterfaceWidget
    {
        public enum ConsoleState
        {
            STATE_OPEN,
            STATE_CLOSE
        };
        GameObject m_ConsoleInput;
        GameObject m_ConsoleText;
        public GameObject m_Console;

        bool m_Activity = false;
        InputField m_LineEdit;
        Text m_Text;
        ConsoleState m_State =  ConsoleState.STATE_CLOSE;

        void Start()
        {
            m_ConsoleInput = GameObject.Find("ConsoleInput");
            m_ConsoleText = GameObject.Find("ConsoleText");

            m_LineEdit = m_ConsoleInput.GetComponent<InputField>();
            m_LineEdit.onEndEdit.AddListener(onConsoleInputEditfinished);
            m_Text = m_ConsoleText.GetComponent<Text>();

            m_Console.SetActive(m_Activity);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void onConsoleInputEditfinished(string text)
        {
            if(text == "")
            {
                return;
            }
            m_LineEdit.text = "";
            m_Text.text += text + "\n";
            string[] command = text.Split(' ');

            GUI.FocusControl("ConsoleInput");

            int commandNumber = command.Length;
            if (commandNumber < 1)
            {
                return;
            }
            if (commandNumber == 2 && command[0] == "add_item")
            {
                int itemId = int.Parse(command[1]);
                string eventName = EventEnum.getEventEnumString(EventEnum.EventEnumType.BACKPACK_ITEM_ADD);
                EventSystem.get().emitEvent(eventName, itemId);
            }
            else if(commandNumber >= 1 && command[0] == "exit")
            {
                Application.Quit();
            }
            else
            {
                m_Text.text +="unkone command \"" + text + "\" \n";
            }
            m_LineEdit.Select();
        }

        public override void show()
        {
            base.show();
            m_Console.SetActive(!isHidden());
            m_State = ConsoleState.STATE_OPEN;
            EventSystem.get().emitEvent(EventEnum.EventEnumType.UI_CONSOLE_STATE_CHANGED_int, (int)m_State);
        }
        public override void hide()
        {
            base.hide();
            m_Console.SetActive(!isHidden());
            m_State = ConsoleState.STATE_CLOSE;
            EventSystem.get().emitEvent(EventEnum.EventEnumType.UI_CONSOLE_STATE_CHANGED_int, (int)m_State);
        }
    }
}
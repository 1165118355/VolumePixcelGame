using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaterBox
{
    public class SceneUIManager : MonoBehaviour
    {
        public ConsoleBox Console;
        public RolePanel RolePanel;
        public MainMenu MainMenu;

        public GameObject RoleB;

        void Start()
        {
            RoleB.GetComponent<Button>().onClick.AddListener(onRoleClicked);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MainMenu.setHidden(!MainMenu.isHidden());
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Console.setHidden(!Console.isHidden());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RolePanel.setHidden(!RolePanel.isHidden());
            }
        }

        protected void onRoleClicked()
        {
            RolePanel.setHidden(!RolePanel.isHidden());
        }

    }
}
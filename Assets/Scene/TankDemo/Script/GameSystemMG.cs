using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WaterBox
{
    public class GameSystemMG
    {
        static public GameSystemMG Instance = new GameSystemMG();
        private List<GameSystem> m_GameSystems;         //  所有的游戏系统都放在这里

        private TerrainSystem       m_TerrainMG;        //  地形系统
        private GlobalConfigSystem  m_GlobalConfig;     //  地形系统
        private ActorSystem         m_ActorSystem;      //  角色系统
        private UISystem            m_UISystem;         //    UI系统
        private SceneManager        m_SceneManager;     //  场景管理
        private ItemSystem          m_ItemSystem;       //  道具管理

        public GameSystemMG()
        {
            m_GameSystems = new List<GameSystem>();
        }

        /// <summary>
        ///     初始化函数
        /// </summary>
        public void init()
        {
            m_GlobalConfig = new GlobalConfigSystem(this);
            m_TerrainMG = new TerrainSystem(this);
            m_UISystem = new UISystem(this);
            m_SceneManager = new SceneManager(this);
            m_ItemSystem = new ItemSystem(this);
            m_ActorSystem = new ActorSystem(this);

            for (int i = 0; i < m_GameSystems.Count(); ++i)
            {
                m_GameSystems[i].init();
            }
            m_GlobalConfig.load();
        }

        /// <summary>
        ///     刷新函数
        /// </summary>
        public void update()
        {
            for (int i = 0; i < m_GameSystems.Count(); ++i)
            {
                m_GameSystems[i].update();
            }
            if (Input.GetKey(KeyCode.F1))
            {
                m_GlobalConfig.save();
            }
            EventSystem.get().update();
        }

        /// <summary>
        ///     结束函数
        /// </summary>
        public void shutdown()
        {
            for (int i = 0; i < m_GameSystems.Count(); ++i)
            {
                m_GameSystems[i].shutdown();
            }
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        ///     添加游戏系统到管理器，这个函数在游戏系统的构造函数中自动调用
        /// </summary>
        /// <param name="gameSystem"></param>
        public void addGameSystem(GameSystem gameSystem)
        {
            m_GameSystems.Add(gameSystem);
        }

        /// <summary>
        ///     发送消息，游戏系统之间的消息传递
        /// </summary>
        /// <param name="message"></param>
        public void sendMessage(String message)
        {
        }

        public int getNumSystem()
        {
            return m_GameSystems.Count;
        }

        public GameSystem getSystem(int index)
        {
            return m_GameSystems[index];
        }
    }
}

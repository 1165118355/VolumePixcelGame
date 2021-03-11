using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WaterBox
{
    class SceneManager : GameSystem
    {
        Timer m_Timer;
        public SceneManager(GameSystemMG manager):base(manager)
        {
            m_Timer = new Timer(manager);
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
    }
}

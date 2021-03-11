using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WaterBox
{
    public class GameSystem
    {
        protected GameSystemMG m_GameSystemMG;

        protected GameSystem(GameSystemMG gameSystemMG)
        {
            m_GameSystemMG = gameSystemMG;
            m_GameSystemMG.addGameSystem(this);
        }

        public virtual void init()
        {
        }

        public virtual void update()
        {
        }

        public virtual void shutdown()
        {
        }

        public virtual void load(XmlNode node)
        {
        }

        public virtual void save(XmlDocument document, XmlElement parent)
        {
        }
    }
}

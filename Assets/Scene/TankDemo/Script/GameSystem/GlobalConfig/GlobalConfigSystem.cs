using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;

namespace WaterBox
{
    public class GlobalConfigSystem :
        GameSystem
    {

        public GlobalConfigSystem(GameSystemMG gameSystemMG):base(gameSystemMG)
        {
            EventSystem.get().registrationEvent(EventEnum.EventEnumType.GLOBALCONFIG_SAVE, save);
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
        public void load()
        {
            string fileName = "./config/globalConfig.cfg";
            if(!File.Exists(fileName))
            {
                UtilsCommon.warning("can not found file " + fileName);
                return;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load(fileName);
            XmlNode globalConfig = xml.SelectSingleNode("global_config");

            int numSystem = m_GameSystemMG.getNumSystem();
            for (int i = 0; i < numSystem; ++i)
            {
                GameSystem system = m_GameSystemMG.getSystem(i);
                system.load(globalConfig);
            }
        }

        public void save()
        {
            XmlDocument xml = new XmlDocument();
            XmlElement globalConfig = xml.CreateElement("", "global_config", "");
            xml.AppendChild(globalConfig);

            int numSystem = m_GameSystemMG.getNumSystem();
            for (int i=0; i< numSystem; ++i)
            {
                GameSystem system = m_GameSystemMG.getSystem(i);
                system.save(xml, globalConfig);
            }

            Directory.CreateDirectory("./config/");
            xml.Save("./config/globalConfig.cfg");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WaterBox
{
    public class ActorSystem : GameSystem
    {
        public ActorSystem(GameSystemMG mG):base(mG)
        {

        }
        public override void init()
        {
            base.init();
        }

        public override void update()
        {
            base.update();
        }
        public override void shutdown()
        {
            base.shutdown();
        }
        public override void save(XmlDocument document, XmlElement parent)
        {
            base.save(document, parent);
            XmlElement actors = document.CreateElement("actors");
            XmlElement master = document.CreateElement("master");
            parent.AppendChild(actors);
        }
        public override void load(XmlNode node)
        {
            base.load(node);
        }
    }
}

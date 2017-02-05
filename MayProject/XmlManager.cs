using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MayProject
{
    class XmlManager
    {
        public static void Save(object obj)
        {
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            using (FileStream file = new FileStream("data.xml", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xml.Serialize(file, obj);
            }
        }

        public static object Load(Type type)
        {
            XmlSerializer xml = new XmlSerializer(type);
            using (FileStream file = new FileStream("data.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return xml.Deserialize(file);
            }
        }
    }
}

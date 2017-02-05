using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using MayProject.Books;

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

        public static Book Load()
        {
            XmlSerializer xml = new XmlSerializer(typeof(Book));
            using (FileStream file = new FileStream("data.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return xml.Deserialize(file) as Book ;
            }
        }
    }
}

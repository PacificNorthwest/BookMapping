using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;
using System.Windows;

namespace MayProject.DataModel
{
    [Serializable]
    public struct LinkInfo
    {
        public string SourceNodeName { get; set; }
        public string DestinationNodeName { get; set; }
        public string LabelText { get; set; }
    }

    [Serializable]
    public class RelationsMap
    {
        public List<string> Elements { get; set; } = new List<string>();
        public XmlSerializableDictionary<string, Point> Coordinates { get; set; } = 
                                                new XmlSerializableDictionary<string, Point>();
        public List<LinkInfo> Links { get; set; } = new List<LinkInfo>();
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;
using System.Drawing;

namespace MayProject.DataModel
{
    [Serializable]
    public class Character : IIllustratable
    {
        public string Title { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Appearence { get; set; }
        public string Description { get; set; }
        public List<byte[]> Illustrations { get; set; } = new List<byte[]>();

        public Character(string title)
        {
            Title = title;
        }

        public Character() : this("No name")
        { }
    }
}

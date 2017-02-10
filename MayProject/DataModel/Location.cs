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
    public class Location : IIllustratable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public List<Bitmap> Illustrations { get; set; } = new List<Bitmap>();

        public Location(string title)
        {
            Title = title;
        }

        public Location() : this("No title")
        { }
    }
}

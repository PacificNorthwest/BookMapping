using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;
using System.Drawing;

namespace MayProject.Books
{
    [Serializable]
    public class Character : IIllustratable
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<Bitmap> Illustrations { get; set; } = new List<Bitmap>();

        public Character(string title)
        {
            Title = title;
        }

        public Character() : this("No name")
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Books
{
    [Serializable]
    public class Character : AbstractIllustratableElement
    {
        public string Description { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();

        public Character(string title)
        {
            Title = title;
            Illustrations = new List<System.Drawing.Bitmap>();
        }

        public Character() : this("No name")
        { }

        public override void AddIllustration(string path)
        {
            throw new NotImplementedException();
        }

        public override void RemoveIllustration()
        {
            throw new NotImplementedException();
        }

        public void AddLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public void RemoveLocation(Location location)
        {
            throw new NotImplementedException();
        }
    }
}

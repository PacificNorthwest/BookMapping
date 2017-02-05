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
        }

        public Character()
        {
            Title = "No name";
        }

        public override void AddIllustration()
        {
            throw new NotImplementedException();
        }

        public override void RemoveIllustration()
        {
            throw new NotImplementedException();
        }
    }
}

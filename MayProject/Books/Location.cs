using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Books
{
    [Serializable]
    public class Location : AbstractIllustratableElement
    {
        public string Description { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();

        public Location(string title)
        {
            Title = title;
            Illustrations = new List<System.Drawing.Bitmap>();
        }

        public Location() : this("No title")
        { }

        public override void AddIllustration(string path)
        {
            throw new NotImplementedException();
        }

        public override void RemoveIllustration()
        {
            throw new NotImplementedException();
        }

        public void AddCharacter(Character character)
        {
            throw new NotImplementedException();
        }

        public void RemoveCharacter(Character character)
        {
            throw new NotImplementedException();
        }
    }
}

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
        }

        public Location()
        {
            Title = "No title";
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

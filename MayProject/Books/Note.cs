using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Books
{
    [Serializable]
    public class Note : AbstractBookElement
    {
        public string Content { get; set; }

        public Note(string title)
        {
            Title = title;
        }

        public Note()
        {
            Title = "No title";
        }
    }
}

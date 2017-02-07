using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.Books
{
    [Serializable]
    public class Chapter : IElement
    {
        public string Title { get; set; }
        public string Annotation { get; set; }
        public string Text { get; set; }

        public Chapter(string title)
        {
            Title = title;
        }

        public Chapter() : this("No title")
        { }
    }
}

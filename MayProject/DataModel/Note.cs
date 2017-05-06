using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.DataModel
{
    [Serializable]
    public class Note : IElement
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public Note() : this("No title", string.Empty)
        { }
    }
}

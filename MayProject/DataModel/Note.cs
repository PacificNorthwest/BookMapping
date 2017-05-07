using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.DataModel
{
    [Serializable]
    public class Note : IPlainTextElement
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public Note(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public Note() : this("No title", string.Empty)
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;

namespace MayProject.DataModel
{
    [Serializable]
    public class Book : IIllustratable
    {
        public string Title { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<Note> Notes { get; set; } = new List<Note>();
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public List<Event> Events { get; set; } = new List<Event>();
        public Map RelationsMap { get; set; } = new Map();
        public Map EventsMap { get; set; } = new Map();
        public List<byte[]> Illustrations { get; set; } = new List<byte[]>();

        public Book(string title) { Title = title; }
        public Book() : this("No title") { }
    }
}

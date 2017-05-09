using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.DataModel
{
    [Serializable]
    public class Event : IElement
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChapterName { get; set; }
        public List<Character> Characters { get; set; } = new List<Character>();
        public Location Location { get; set; } = new Location();

        public Event(string title, string description, string chapterName, List<Character> characters, Location location)
        {
            Title = title;
            Description = description;
            ChapterName = chapterName;
            Characters = characters;
            Location = location;
        }

        public Event(string title) : this(title, "No description", "No chapter name", new List<Character>(), new Location()) { }

        public Event() : this("No title", "No description", "No chapter name", new List<Character>(), new Location()) { }
    }
}

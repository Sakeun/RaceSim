using System;
namespace Model
{
    public class Track
    {
        public string Name;
        public LinkedList<Section> Sections;
        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
        }
    }
}


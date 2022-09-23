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
            Sections = SectionTypestoList(sections);
        }

        private LinkedList<Section> SectionTypestoList(SectionTypes[] types)
        {
            LinkedList<Section> list = new LinkedList<Section>();
            foreach(SectionTypes section in types)
            {
                list.AddLast(new Section(section));
            }

            return list;
        }
    }
}


﻿using System.Timers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Model;
using Timer = System.Timers.Timer;

namespace Controller
{
    [TestFixture]
    public class Race_Tests
    {
        private Race _raceTest;

        [SetUp]
        public void SetUp()
        {
            Competition comp = new Competition();
            Data.Initialize(comp);
			Data.NextRace();
			_raceTest = new Race(Data.CurrentRace.Track, Data.CurrentRace.Participants);
        }

        [Test]
        public void GetSectionData_NewSection_ReturnSection()
        {
			Section section = Data.CurrentRace.Track.Sections.First.Value;
			SectionData sectionData = _raceTest.GetSectionData(section, Data.CurrentRace.Participants[0], Data.CurrentRace.Participants[1], 40, 80);
			Assert.NotNull(sectionData);
		}

        [Test]
        public void MovePlayerNextSection_ReturnNext()
        {
            Section current = Data.CurrentRace.Track.Sections.ElementAt(0);
            Section next = Data.CurrentRace.Track.Sections.ElementAt(1);

            Section result = Data.CurrentRace.MovePlayerNextSection(current, next, Data.CurrentRace.Participants[0], 1000, false);

            Assert.That(result.SectionType, Is.EqualTo(next.SectionType));
        }

        [Test]
        public void CreateStack_ReturnAllSections()
        {
            int expected = Data.CurrentRace.Track.Sections.Count;
            Stack<Section> result = Data.CurrentRace.CreateStack();

            Assert.That(expected, Is.EqualTo(result.Count));
        }

    }
    
}
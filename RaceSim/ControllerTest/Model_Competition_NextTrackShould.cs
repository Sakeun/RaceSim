using System;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        public Model_Competition_NextTrackShould()
        {
        }
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            SectionTypes[] types = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish };
            Track t1 = new Track("Track1", types);
            _competition.Tracks.Enqueue(t1);

            var result = _competition.NextTrack();
            Assert.AreEqual(t1, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] types = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish };
            Track t1 = new Track("Track2", types);
            _competition.Tracks.Enqueue(t1);

            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] types = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.StartGrid, SectionTypes.Finish };
            Track t1 = new Track("Track1", types);
            _competition.Tracks.Enqueue(t1);
            Track t2 = new Track("Track2", types);
            _competition.Tracks.Enqueue(t2);

            var result = _competition.NextTrack();
            Assert.AreEqual(result, t1);
            result = _competition.NextTrack();
            Assert.AreEqual(result, t2);
        }
    }
}


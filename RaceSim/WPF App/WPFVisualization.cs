using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace WPF_App
{
    public static class WPFVisualization
    {
        private const string _straightTrack = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Straight_Field.png";
        private const string _corner = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\corner_field.png";

        private const string _anakin = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Anakin Pod.png";
        private const string _anakinBroken = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Anakin pod broken.png";

        private static int _imageSize = 320;

        public static BitmapSource DrawTrack()
        {
            Bitmap map = ImageCreator.GetBitmap(_straightTrack);

            BitmapSource bmp = ImageCreator.CreateBitmapSourceFromGdiBitmap(map);

            return bmp;
        }

        public static BitmapSource DrawTrack(Track track)
        {
            int[] lowestValxy = SetLowestVal(track);
            int[] totalxy = SetTotalVal(track);
            Trace.WriteLine($"{totalxy[0]} + {totalxy[1]}");
            Bitmap map = ImageCreator.GetBitmap(_imageSize * totalxy[0], _imageSize * totalxy[1]);

            Graphics graphic = Graphics.FromImage(map);
            int compass = 0;

            foreach (Section section in track.Sections)
            {
                int xdir = section.x + Math.Abs(lowestValxy[0]);
                int ydir = section.y + Math.Abs(lowestValxy[1]);
                //DrawableTrack = CurrentTrack(section.SectionType, compass);
                DrawSections(section, xdir, ydir, graphic, compass);
                compass = SetCompass(compass, section);
                //Data.CurrentRace.stck.Push(section);
            }

            return ImageCreator.CreateBitmapSourceFromGdiBitmap(map);
        }

        private static int SetCompass(int dir, Section sec)
        {
            if (sec.SectionType == SectionTypes.RightCorner)
            {
                dir++;
            }
            else if (sec.SectionType == SectionTypes.LeftCorner)
            {
                dir--;
            }
            if (dir == 4)
            {
                dir = 0;
            }
            else if (dir == -1)
            {
                dir = 3;
            }

            return dir;
        }

        public static int[] SetLowestVal(Track track)
        {
            int[] xy = { 0, 0 };
            int[] lowestValxy = { 0, 0 };
            int compass = 0;
            foreach (Section section in track.Sections)
            {
                xy = section.SetPosition(xy, compass);
                compass = SetCompass(compass, section);
                if (section.x < lowestValxy[0])
                {
                    lowestValxy[0] = section.x;
                }
                if (section.y < lowestValxy[1])
                {
                    lowestValxy[1] = section.y;
                }
            }

            return lowestValxy;
        }

		public static int[] SetTotalVal(Track track)
		{
			int[] xy = { 0, 0 };
			int[] highestvalxy = { 1, 1 };
            int[] lastSectionxy = { 0, 0 };
			int compass = 0;
			foreach (Section section in track.Sections)
			{
				xy = section.SetPosition(xy, compass);
				compass = SetCompass(compass, section);
				if (section.x > lastSectionxy[0])
				{
					highestvalxy[0]++;
				}
				if (section.y > lastSectionxy[1])
				{
					highestvalxy[1]++;
				}
                lastSectionxy[0] = section.x;
                lastSectionxy[1] = section.y;
			}

			return highestvalxy;
		}

		private static void DrawSections(Section section, int x, int y, Graphics graph, int compass)
        {
            Bitmap mp = GetCurrentMap(section, compass);
            mp = DrawUser(section, mp, compass);

            graph.DrawImage(mp, new Point(x * _imageSize, y * _imageSize));
        }

        private static Bitmap GetCurrentMap(Section section, int compass)
        {
            Bitmap map;
            if((section.SectionType == SectionTypes.LeftCorner && compass == 1) || (section.SectionType == SectionTypes.RightCorner && compass == 2))
            {
                map = new Bitmap(_corner);
                map.RotateFlip(RotateFlipType.Rotate180FlipNone);
                return map;
            }
            if((section.SectionType == SectionTypes.LeftCorner && compass == 0) || (section.SectionType == SectionTypes.RightCorner && compass == 1))
            {
                map = new Bitmap(_corner);
                map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                return map;
            }
            if((section.SectionType == SectionTypes.LeftCorner && compass == 2) || (section.SectionType == SectionTypes.RightCorner && compass == 3))
            {
                map = new Bitmap(_corner);
                map.RotateFlip(RotateFlipType.Rotate270FlipNone);
                return map;
            }

            if((section.SectionType != SectionTypes.LeftCorner && section.SectionType != SectionTypes.RightCorner) && (compass == 1 || compass == 3))
            {
                map = new Bitmap(_straightTrack);
                map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                return map;
            } else if(section.SectionType != SectionTypes.LeftCorner && section.SectionType != SectionTypes.RightCorner)
            {
                return new Bitmap(_straightTrack);
            }

            return new Bitmap(_corner);
        }

        private static Bitmap DrawUserOnMap(Bitmap map, SectionData section, IParticipant[] participants)
        {
            Graphics g = Graphics.FromImage(map);
            g.CompositingMode = CompositingMode.SourceOver;
            Image img = Image.FromFile(_anakin);
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);

            //SectionData data = Data.CurrentRace.GetSectionData(section);
            //if(section != null && section.Left != null)
            //{
            //    g.DrawImage(img, 2, 2, 250, 250);
            //}
            foreach(var participant in participants)
            {
                if (section == null) break;
                //if()
                if(section.Left != null && section.Left == participant)
                {
                    g.DrawImage(img, 2, 2, 250, 250);
                } else if(section.Right != null && section.Right == participant)
                {
                    g.DrawImage(img, 2, 200, 250, 250);
                }
            }
            g.Dispose();

            return map;
        }

        public static Bitmap DrawUser(Section section, Bitmap map, int compass)
        {
            SectionData sectionD = Data.CurrentRace.GetSectionData(section);

            IParticipant[] participants = Data.CurrentRace.Participants.ToArray();

            Data.CurrentRace.MovePlayer(participants);

			Graphics g = Graphics.FromImage(map);

			g.CompositingMode = CompositingMode.SourceOver;

			Image img = Image.FromFile(_anakin);


            switch(compass)
            {
                case 0:
					img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 1:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;

			}
			foreach (var participant in participants)
			{
                if(participant.Equipment.IsBroken)
                {
                    img = Image.FromFile(_anakinBroken);
                }
				if (sectionD == null) break;
				
				if (sectionD.Left != null && sectionD.Left == participant)
				{
					g.DrawImage(img, 2, 2, 250, 250);
				}
				else if (sectionD.Right != null && sectionD.Right == participant)
				{
					g.DrawImage(img, 2, 200, 250, 250);
				}
			}
			g.Dispose();

			return map;
			//return DrawUserOnMap(map, d, participants);
        }
    }
}

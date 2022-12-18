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
        private const string _finishTrack = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Finish_field.png";
        private const string _startTrack = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\starting_grid.png";

        private const string _anakin = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Anakin Pod.png";
        private const string _anakinBroken = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Anakin pod broken.png";
		private const string _sebulba = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Sebulba Pod.png";
		private const string _sebulbaBroken = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Sebulba pod broken.png";
		private const string _nemesso = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Nemesso pod.png";
		private const string _nemessoBroken = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Nemesso pod broken.png";
		private const string _quadinaros = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Quadinaros pod.png";
		private const string _quadinarosBroken = "Z:\\Documents\\RaceSim\\RaceSim\\WPF App\\Assets\\Quadinaros pod broken.png";

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
            Bitmap map = ImageCreator.GetBitmap(_imageSize * totalxy[0], _imageSize * totalxy[1]);

            Graphics graphic = Graphics.FromImage(map);
            int compass = 0;

            foreach (Section section in track.Sections)
            {
                int xdir = section.x + Math.Abs(lowestValxy[0]);
                int ydir = section.y + Math.Abs(lowestValxy[1]);
                DrawSections(section, xdir, ydir, graphic, compass);
                compass = SetCompass(compass, section);
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

            if((section.SectionType == SectionTypes.Straight) && (compass == 1 || compass == 3))
            {
                map = new Bitmap(_straightTrack);
                map.RotateFlip(RotateFlipType.Rotate90FlipNone);
                return map;
            } 
            if((section.SectionType == SectionTypes.Finish) && (compass == 1 || compass == 3))
            {
                map = new Bitmap(_finishTrack);
				map.RotateFlip(RotateFlipType.Rotate90FlipNone);
				return map;
			}
			if((section.SectionType == SectionTypes.StartGrid) && (compass == 1 || compass == 3))
			{
				map = new Bitmap(_startTrack);
				map.RotateFlip(RotateFlipType.Rotate90FlipNone);
				return map;
			} 
            if(section.SectionType == SectionTypes.Straight)
            {
                return new Bitmap(_straightTrack);
            }
			if(section.SectionType == SectionTypes.Finish)
			{
				return new Bitmap(_finishTrack);
			}
			if(section.SectionType == SectionTypes.StartGrid)
			{
				return new Bitmap(_startTrack);
			}

			return new Bitmap(_corner);
        }

        public static Bitmap DrawUser(Section section, Bitmap map, int compass)
        {
            SectionData sectionD = Data.CurrentRace.GetSectionData(section);

            IParticipant[] participants = Data.CurrentRace.Participants.ToArray();

            Data.CurrentRace.MovePlayer(participants);

			Graphics g = Graphics.FromImage(map);

			g.CompositingMode = CompositingMode.SourceOver;

			Image img = Image.FromFile(_anakin);

			foreach (var participant in participants)
			{
                img = Image.FromFile(GetCurrentPlayerPath(participant));
                img.RotateFlip(GetVisualPlayerRotation(compass));
				if (sectionD == null) break;
				if (sectionD.Left != null && sectionD.Left == participant)
				{
                    if ((section.SectionType == SectionTypes.Straight && compass == 1))
                    {
                        g.DrawImage(img, 200, 2, 250, 250);
                    }
                    else
                    {
                        g.DrawImage(img, 2, 2, 250, 250);
                    }
				}
				else if (sectionD.Right != null && sectionD.Right == participant)
				{
                    if ((section.SectionType == SectionTypes.RightCorner && compass == 2) || (section.SectionType == SectionTypes.Straight && compass == 3))
                    {
                        g.DrawImage(img, 200, 2, 250, 250);
                    }
                    else
                    {
                        g.DrawImage(img, 2, 200, 250, 250);
                    }
				}
			}
			g.Dispose();

			return map;
        }

        private static RotateFlipType GetVisualPlayerRotation(int compass)
        {
			switch (compass)
			{
				case 0:
					return RotateFlipType.RotateNoneFlipX;
				case 1:
					return RotateFlipType.Rotate270FlipNone;
				case 3:
					return RotateFlipType.Rotate90FlipNone;
                default:
                    return RotateFlipType.RotateNoneFlipNone;
			}
		}

        private static string GetCurrentPlayerPath(IParticipant participant)
        {
            if(participant.Name == "Anakin" && !participant.Equipment.IsBroken)
            {
                return _anakin;
            }
            else if(participant.Name == "Anakin" && participant.Equipment.IsBroken)
            {
                return _anakinBroken;
            }
			else if (participant.Name == "Sebulba" && !participant.Equipment.IsBroken)
			{
				return _sebulba;
			}
			else if (participant.Name == "Sebulba" && participant.Equipment.IsBroken)
			{
				return _sebulbaBroken;
			}
			else if (participant.Name == "Nemesso" && !participant.Equipment.IsBroken)
			{
				return _nemesso;
			}
			else if (participant.Name == "Nemesso" && participant.Equipment.IsBroken)
			{
				return _nemessoBroken;
			}
			else if (participant.Name == "Quadinaros" && !participant.Equipment.IsBroken)
			{
				return _quadinaros;
			}
			else if (participant.Name == "Quadinaros" && participant.Equipment.IsBroken)
			{
				return _quadinarosBroken;
			}

            return "";
		}
    }
}

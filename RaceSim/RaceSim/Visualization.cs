using Controller;
using Model;
namespace RaceSim;

public static class Visualization
{
    public static void Initialize()
    {
        Data.CurrentRace.DriversChanged += OnDriversChanged;
    }

    // public static int[] SetConsolePositions(Track track)
    // {
    //
    //     return LowestValxy;
    // }

    public static void DrawTrack(Track track, IParticipant[] users)
    {
        Console.SetCursorPosition(0, 0);
        int[] xy = { 0 , 0 };
        int compass = 0;
        int[] lowestValxy = { 0, 0 };
        int userint = 0;

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
        
        string[] DrawableTrack = new string[5];
        compass = 0;
        foreach (Section section in track.Sections)
        {
            int xdir = section.x + Math.Abs(lowestValxy[0]);
            int ydir = section.y + Math.Abs(lowestValxy[1]);
            DrawableTrack = CurrentTrack(section.SectionType, compass);
            DrawableTrack = DrawUser(DrawableTrack, users[userint], users[userint + 1], section);
            PrintTrack(xdir * 5, ydir * 5, DrawableTrack);
            compass = SetCompass(compass, section);
            //Data.CurrentRace.stck.Push(section);
        }
    }

    private static string[] CurrentTrack(SectionTypes type, int dir)
    {
        string[] printable;

        if ((type.Equals(SectionTypes.LeftCorner) && dir == 1) || (type.Equals(SectionTypes.RightCorner) && dir == 2))
        {
            printable = _reverseLeftC;
        } else if ((type.Equals(SectionTypes.LeftCorner) && dir == 3) ||
                   (type.Equals(SectionTypes.RightCorner) && dir == 0))
        {
            printable = _trackRightC;
        } else if ((type.Equals(SectionTypes.LeftCorner) && dir == 2) ||
                   (type.Equals(SectionTypes.RightCorner) && dir == 3))
        {
            printable = _reverseRightC;
        } else if ((type.Equals(SectionTypes.LeftCorner) && dir == 0) ||
                   (type.Equals(SectionTypes.RightCorner) && dir == 1))
        {
            printable = _trackLeftC;
        } else if (type.Equals(SectionTypes.Straight) && (dir == 1 || dir == 3))
        {
            printable = _trackVertical;
        } else if (type.Equals(SectionTypes.Finish))
        {
            printable = _finishHorizontal;
        } else if (type.Equals(SectionTypes.StartGrid))
        {
            printable = _startingGrid;
        }
        else
        {
            printable = _trackStraight;
        }

        return printable;
    }

    private static string[] DrawUser(string[] startGrid, IParticipant left, IParticipant right, Section section)
    {
        string[] temp = new string[5];
        startGrid.CopyTo(temp, 0);
        SectionData data = Data.CurrentRace.GetSectionData(section);
        for (int i = 0; i < startGrid.Length; i++)
        {
            if (data != null && data.Left != null)
            {
                temp[i] = temp[i].Replace("1", data.Left.Name.Substring(0, 1));
            } else
            {
                temp[i] = temp[i].Replace("1", " ");
            }
            if (data != null && data.Right != null)
            {
                temp[i] = temp[i].Replace("2", data.Right.Name.Substring(0, 1));
            }
            else
            {
                temp[i] = temp[i].Replace("2", " ");
            }
        }

        return temp;
    }

    private static void PrintTrack(int xax, int yax, string[] track)
    {
        Console.SetCursorPosition(xax, yax);
        foreach (string p in track)
        {
            Console.Write(p);
            yax++;
            Console.SetCursorPosition(xax, yax);
        }
    }

    public static void OnDriversChanged(object? sender, DriversChangedEventArgs e)
    {
        Data.CurrentRace.MovePlayer(e.Users);
        DrawTrack(Data.CurrentRace.Track, e.Users);
    }

    private static int SetCompass(int dir, Section sec)
    {
        if (sec.SectionType == SectionTypes.RightCorner)
        {
            dir++;
        } else if (sec.SectionType == SectionTypes.LeftCorner)
        {
            dir--;
        }
        if (dir == 4)
        {
            dir = 0;
        } else if (dir == -1)
        {
            dir = 3;
        }

        return dir;
    }
    
    #region graphics
    private static string[] _finishHorizontal =
    {
        "-----", 
        "1  FI", 
        "2  FI", 
        "-----", 
        "     "
    };
    private static string[] _startingGrid =
    {
        "-----", 
        " #1  ", 
        "   #2", 
        "-----", 
        "     "
    };
    
    private static string[] _trackStraight =
    {
        "-----", 
        "1    ", 
        "2    ", 
        "-----", 
        "     "
    };
    private static string[] _trackVertical =
    {
        "|1 2|", 
        "|   |", 
        "|   |", 
        "|   |", 
        "|   |"
    };
    
    private static string[] _trackRightC =
    {
        "----\\", 
        "1   |", 
        "2   |", 
        "\\   |", 
        "|   |"
    };
    private static string[] _reverseRightC =
    {
        "/----", 
        "|    ", 
        "|    ", 
        "|   /", 
        "|1 2|"
    };

    private static string[] _trackLeftC =
    {
        "/   |", 
        "1   |", 
        "2   |", 
        "----/", 
        "     "
    };
    private static string[] _reverseLeftC =
    {
        "|  \\-" , 
        "|   1", 
        "|   2", 
        "\\----", 
        "     "
    };

    #endregion
}
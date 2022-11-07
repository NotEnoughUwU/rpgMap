using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpgMap
{
    internal class Program
    {
        static char[,] map = new char[,] // dimensions defined by following data:
        {
            {'^','^','^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'^','^','`','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`'},
            {'^','^','`','`','`','*','*','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','~','~','~','`','`','`','`','`'},
            {'^','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','`','`','`','`','`','`'},
            {'`','`','`','`','`','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`','`','`'},
            {'`','`','`','`','`','~','~','~','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','^','^','^','^','`','`','`'},
            {'`','`','`','`','`','`','`','~','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
            {'`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`','`'},
        };
        // map legend:
        // ^ = mountain
        // ` = grass
        // ~ = water
        // * = trees

        static int mapScale = 6;
        
        static void Main(string[] args)
        {
            //SetScale();
            DrawMap();
            DrawBorder();
            
            Console.ReadKey(true);
        }

        static void SetScale()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.Write("INPUT MAP SCALE INTEGER: ");

            try
            {
                char mapScaleChar = Console.ReadKey().KeyChar;
                Console.WriteLine(mapScale);
                Console.ReadKey(true);
            }
            catch (Exception)
            {
                SetScale();
            }
        }

        static void ScaleWindow()
        {
            Console.WindowWidth = map.GetLength(1) * mapScale + 2;
        }

        static void DrawMap()
        {
            ScaleWindow();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    SetColour(c, i);

                    for (int s = 0; s < mapScale; s++)
                    {
                        for (int s2 = 0; s2 < mapScale; s2++)
                        {
                            Console.SetCursorPosition(c * mapScale + 1 + s, i * mapScale + 1 + s2);
                            Console.WriteLine(map[i, c]);
                        }
                    }
                }
            }
        }

        static void SetColour(int c, int i)
        {
            switch (map[i, c])
            {
                case '^':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case '`':
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case '~':
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case '*':
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
            }
        }

        static void DrawBorder()
        {
            Console.BackgroundColor= ConsoleColor.Black;
            Console.ForegroundColor= ConsoleColor.White;

            for (int i = 0; i < map.GetLength(1) * mapScale; i++)
            {
                Console.SetCursorPosition(i  + 1, 0);
                Console.WriteLine('═');
            }
            for (int i = 0; i < map.GetLength(1) * mapScale; i++)
            {
                Console.SetCursorPosition(i + 1, map.GetLength(0) * mapScale + 1);
                Console.WriteLine('═');
            }
            for (int i = 0; i < map.GetLength(0) * mapScale; i++)
            {
                Console.SetCursorPosition(0, i + 1);
                Console.WriteLine('║');
            }
            for (int i = 0; i < map.GetLength(0) * mapScale; i++)
            {
                Console.SetCursorPosition(map.GetLength(1) * mapScale + 1, i + 1);
                Console.WriteLine('║');
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine('╔');
            Console.SetCursorPosition(map.GetLength(1) * mapScale + 1, 0);
            Console.WriteLine('╗');
            Console.SetCursorPosition(0, map.GetLength(0) * mapScale + 1);
            Console.WriteLine('╚');
            Console.SetCursorPosition(map.GetLength(1) * mapScale + 1, map.GetLength(0) * mapScale  + 1);
            Console.WriteLine('╝');
        }
    }
}

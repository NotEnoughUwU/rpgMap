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

        static int mapScale;

        static int playerX;
        static int playerY;
        static int subX;
        static int subY;

        static char player = '☻';
        
        static void Main(string[] args)
        {
            ClearInputBuffer();
            SetScale();
            InitPlayer();
            InputLoop();
            Console.ReadKey(true);
        }

        static void SetScale()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.Write("INPUT MAP SCALE INTEGER:");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.White;

            char mapScaleChar = Console.ReadKey().KeyChar;
            mapScale = (int)Char.GetNumericValue(mapScaleChar);
            Console.ReadKey(true);

            ScaleWindow();
        }

        static void ScaleWindow()
        {
            try
            {
                Console.WindowWidth = map.GetLength(1) * mapScale + 2;
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Given value either not an integer or too large to scale to");
                Console.ReadKey(true);
                SetScale();
                return;
            }
            DrawMap();
            DrawBorder();
        }

        static void DrawMap()
        {
            Console.ForegroundColor = ConsoleColor.White;

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

        static void InitPlayer()
        {
            playerX = 7;
            playerY = 6;
            subX = 0;
            subY = 0;
            SetPlayerPos();
        }

        static void SetPlayerPos()
        {
            Console.SetCursorPosition((playerX + 1) * mapScale - subX, (playerY + 1) * mapScale - subY);
            SetColour(playerX, playerY);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(player);
        }

        static void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        static void InputLoop()
        {
            while (true)
            {
                char inputChar = Console.ReadKey(true).KeyChar;

                switch (inputChar)
                {
                    case 'w':
                        MoveUp();
                        break;
                    case 's':
                        MoveDown();
                        break;
                    case 'a':
                        MoveLeft();
                        break;
                    case 'd':
                        MoveRight();
                        break;
                }
            }
        }

        static void MoveUp()
        {
            FillOldPos();
            subY++;
            if (subY < 0 || subY >= mapScale)
            {
                if (playerY - 1 <= map.GetLength(1) * -1 || TestCollisionUp())
                {
                    subY--;
                    SetPlayerPos();
                    return;
                }

                subY = 0;
                playerY--;
            }
            SetPlayerPos();
        }
        static void MoveDown()
        {
            FillOldPos();
            subY--;
            if (subY < 0 || subY >= mapScale)
            {
                if (playerY + 1 >= map.GetLength(0) || TestCollisionDown())
                {
                    subY++;
                    SetPlayerPos();
                    return;
                }

                subY = mapScale - 1;
                playerY++;
            }
            SetPlayerPos();
        }
        static void MoveLeft()
        {
            FillOldPos();
            subX++;
            if (subX < 0 || subX >= mapScale)
            {
                if (playerX - 1 <= map.GetLength(0) * -1 || TestCollisionLeft())
                {
                    subX--;
                    SetPlayerPos();
                    return;
                }

                subX = 0;
                playerX--;
            }
            SetPlayerPos();
        }
        static void MoveRight()
        {
            FillOldPos();
            subX--;
            if (subX < 0 || subX >= mapScale || TestCollisionRight())
            {
                if (playerX + 1 >= map.GetLength(1))
                {
                    subX++;
                    SetPlayerPos();
                    return;
                }

                subX = mapScale - 1;
                playerX++;
            }
            SetPlayerPos();
        }

        static bool TestCollisionUp()
        {
            switch (map[playerY - 1, playerX])
            {
                case '^':
                case '~':
                    return true;
                default:
                    return false;
            }
        }
        static bool TestCollisionDown()
        {
            switch (map[playerY + 1, playerX])
            {
                case '^':
                case '~':
                    return true;
                default:
                    return false;
            }
        }
        static bool TestCollisionLeft()
        {
            switch (map[playerY, playerX - 1])
            {
                case '^':
                case '~':
                    return true;
                default:
                    return false;
            }
        }
        static bool TestCollisionRight()
        {
            switch (map[playerY, playerX + 1])
            {
                case '^':
                case '~':
                    return true;
                default:
                    return false;
            }
        }

        static void FillOldPos()
        {
            Console.SetCursorPosition((playerX + 1) * mapScale - subX, (playerY + 1) * mapScale - subY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(map[playerY, playerX]);
        }
    }
}
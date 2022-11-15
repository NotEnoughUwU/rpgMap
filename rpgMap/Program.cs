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

        static char[,] dragon = new char[,]
        {
            {'X','o','X'},
            {'>','O','<'},
            {'X','V','X'},
        };
        static int dragonX;
        static int dragonY;
        static int dragonSubX;
        static int dragonSubY;
        static char[,] dragonCovers = new char[,]
        {
            {'X','X','X'},
            {'X','X','X'},
            {'X','X','X'},
        };
        static bool dragonMoved;

        static int mapScale;

        static int playerX;
        static int playerY;
        static int playerSubX;
        static int playerSubY;

        static char player = '☻';
        
        static void Main(string[] args)
        {
            ClearInputBuffer();
            SetScale();
            InitPlayer();
            InitDragon();
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
            playerSubX = 0;
            playerSubY = 0;
            SetPlayerPos();
        }

        static void SetPlayerPos()
        {
            Console.SetCursorPosition((playerX + 1) * mapScale - playerSubX, (playerY + 1) * mapScale - playerSubY);
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

                DragonAI();
            }
        }

        static void MoveUp()
        {
            FillOldPos();
            playerSubY++;
            if (playerSubY < 0 || playerSubY >= mapScale)
            {
                if (TestCollisionUp())
                {
                    playerSubY--;
                    SetPlayerPos();
                    return;
                }

                playerSubY = 0;
                playerY--;
            }
            SetPlayerPos();
        }
        static void MoveDown()
        {
            FillOldPos();
            playerSubY--;
            if (playerSubY < 0 || playerSubY >= mapScale)
            {
                if (TestCollisionDown())
                {
                    playerSubY++;
                    SetPlayerPos();
                    return;
                }

                playerSubY = mapScale - 1;
                playerY++;
            }
            SetPlayerPos();
        }
        static void MoveLeft()
        {
            FillOldPos();
            playerSubX++;
            if (playerSubX < 0 || playerSubX >= mapScale)
            {
                if (TestCollisionLeft())
                {
                    playerSubX--;
                    SetPlayerPos();
                    return;
                }

                playerSubX = 0;
                playerX--;
            }
            SetPlayerPos();
        }
        static void MoveRight()
        {
            FillOldPos();
            playerSubX--;
            if (playerSubX < 0 || playerSubX >= mapScale)
            {
                if (TestCollisionRight())
                {
                    playerSubX++;
                    SetPlayerPos();
                    return;
                }

                playerSubX = mapScale - 1;
                playerX++;
            }
            SetPlayerPos();
        }

        static bool TestCollisionUp()
        {
            try
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
            catch (Exception)
            {
                return true;
            }
        }
        static bool TestCollisionDown()
        {
            try
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
            catch (Exception)
            {
                return true;
            }
        }
        static bool TestCollisionLeft()
        {
            try
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
            catch (Exception)
            {
                return true;
            }
        }
        static bool TestCollisionRight()
        {
            try
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
            catch (Exception)
            {
                return true;
            }
        }

        static void FillOldPos()
        {
            Console.SetCursorPosition((playerX + 1) * mapScale - playerSubX, (playerY + 1) * mapScale - playerSubY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(map[playerY, playerX]);
        }

        static void InitDragon()
        {
            dragonX = 20;
            dragonY = 8;
            dragonSubX = 0;
            dragonSubY = 0;
            SetDragonPos();
            dragonMoved = true;
        }

        static void SetDragonPos()
        {
            Console.SetCursorPosition((dragonX + 1) * mapScale - dragonSubX, (dragonY + 1) * mapScale - dragonSubY);
            DrawDragon();
        }

        static void DrawDragon()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            for (int i = 0; i < dragon.GetLength(0); i++)
            {
                for (int j = 0; j < dragon.GetLength(1); j++)
                {
                    Console.SetCursorPosition((dragonX + 1) * mapScale - dragonSubX + i, (dragonY + 1) * mapScale - dragonSubY + j);

                    if (dragon[j, i] != 'X')
                    {
                        Console.Write(dragon[j, i]);
                    }
                }
            }
        }

        static void DragonLeft()
        {
            dragonSubX++;
            if (dragonSubX < 0 || dragonSubX >= mapScale)
            {
                dragonSubX = 0;
                dragonX--;
            }
            SetDragonPos();
        }
        static void DragonRight()
        {
            dragonSubX--;
            if (dragonSubX < 0 || dragonSubX >= mapScale)
            {
                dragonSubX = mapScale - 1;
                dragonX++;
            }
            SetDragonPos();
        }
        static void DragonUp()
        {
            dragonSubY++;
            if (dragonSubY < 0 || dragonSubY >= mapScale)
            {
                dragonSubY = 0;
                dragonY--;
            }
            SetDragonPos();
        }
        static void DragonDown()
        {
            dragonSubY--;
            if (dragonSubY < 0 || dragonSubY >= mapScale)
            {
                dragonSubY = mapScale - 1;
                dragonY++;
            }
            SetDragonPos();
        }

        static void DragonAI()
        {
            if (dragonMoved)
            {
                dragonMoved = false;
                return;
            }

            if (playerX > dragonX)
                DragonRight();
            else if (playerX < dragonX)
                DragonLeft();
            else if (playerY > dragonY)
                DragonDown();
            else if (playerY < dragonY)
                DragonUp();

            dragonMoved = true;
        }
    }
}
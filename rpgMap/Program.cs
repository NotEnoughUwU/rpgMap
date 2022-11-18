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

        static char goblin = '☺';
        static int goblinX;
        static int goblinY;
        static int goblinSubX;
        static int goblinSubY;
        static bool goblinMoved;

        static int mapScale;

        static int playerX;
        static int playerY;
        static int playerSubX;
        static int playerSubY;

        static char player = '☻';
        
        static void Main()
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

                GoblinAI();
                if (TestPlayerDie())
                    break;
            }
            Main();
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
            SetColour(playerX, playerY);
            Console.Write(map[playerY, playerX]);
        }

        static void InitDragon()
        {
            goblinX = 20;
            goblinY = 8;
            goblinSubX = 0;
            goblinSubY = 0;
            SetGoblinPos();
            goblinMoved = true;
        }

        static void SetGoblinPos()
        {
            Console.SetCursorPosition((goblinX + 1) * mapScale - goblinSubX, (goblinY + 1) * mapScale - goblinSubY);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(goblin);
        }

        static void GoblinLeft()
        {
            GoblinFillOldPos();

            goblinSubX++;
            if (goblinSubX < 0 || goblinSubX >= mapScale)
            {
                goblinSubX = 0;
                goblinX--;
            }
            SetGoblinPos();
        }
        static void GoblinRight()
        {
            GoblinFillOldPos();

            goblinSubX--;
            if (goblinSubX < 0 || goblinSubX >= mapScale)
            {
                goblinSubX = mapScale - 1;
                goblinX++;
            }
            SetGoblinPos();
        }
        static void GoblinUp()
        {
            GoblinFillOldPos();

            goblinSubY++;
            if (goblinSubY < 0 || goblinSubY >= mapScale)
            {
                goblinSubY = 0;
                goblinY--;
            }
            SetGoblinPos();
        }
        static void GoblinDown()
        {
            GoblinFillOldPos();

            goblinSubY--;
            if (goblinSubY < 0 || goblinSubY >= mapScale)
            {
                goblinSubY = mapScale - 1;
                goblinY++;
            }
            SetGoblinPos();
        }

        static void GoblinFillOldPos()
        {
            Console.SetCursorPosition((goblinX + 1) * mapScale - goblinSubX, (goblinY + 1) * mapScale - goblinSubY);
            Console.ForegroundColor = ConsoleColor.White;
            SetColour(goblinX, goblinY);
            Console.Write(map[goblinY, goblinX]);
        }

        static void GoblinAI()
        {
            if (goblinMoved)
            {
                goblinMoved = false;
                return;
            }

            if (playerX > goblinX)
                GoblinRight();
            else if (playerX < goblinX)
                GoblinLeft();
            else if (playerY > goblinY)
                GoblinDown();
            else if (playerY < goblinY)
                GoblinUp();
            else if (playerX == goblinX && playerY == goblinY)
            {
                if (playerSubX > goblinSubX)
                    GoblinRight();
                else if (playerSubX < goblinSubX)
                    GoblinLeft();
                else
                {
                    if (playerSubY > goblinSubY)
                        GoblinDown();
                    else if (playerSubY < goblinSubY)
                        GoblinUp();
                }
            }

            goblinMoved = true;
        }

        static bool TestPlayerDie()
        {
            if (playerX == goblinX && playerY == goblinY && playerSubX == goblinSubX && playerSubY == goblinSubY)
            {
                GameOver();
                return true;
            }
            return false;
        }
        static void GameOver()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.SetCursorPosition(0,0);
            Console.WriteLine();
            Console.WriteLine("GAME OVER");
            Console.WriteLine("Press any button to play again");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
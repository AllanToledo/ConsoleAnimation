using System;
using System.Threading;
using System.Collections.Generic;

namespace Tetris
{
    class Program
    {

        static char[,] displaySaved;
        static int horizonalMoviment = 0;
        static int verticalMoviment = 0;
        static bool rotate = false;
        static string title = "";
        static char[,] piece;
        static Random random = new Random();
        static int pieceY , pieceX;
        static int frame = 0;
        static int height = 20, width = 10;

        static void Main(string[] args)
        {
            
            char[,] display = new char[height, width];
            RestartPiece();
            InitalizeTitle(width);
            Console.WindowWidth = width * 3;
            Console.Title = "Tetris";
            Initalize(display);
            new Thread(Gravity).Start();
            new Thread(Control).Start();

            while (true)
            {
                Clear(display);

                pieceY += verticalMoviment;
                pieceX += horizonalMoviment;
                if (rotate)
                {
                    piece = Rotate(piece);
                }
                int response = Move(piece, pieceY, pieceX, display);
                if (response == -2 && horizonalMoviment == 0 && !rotate)
                {
                    //the piece find other piece in vertical moviment
                    Clear(display);
                    pieceY++;
                    Move(piece, pieceY, pieceX, display);
                    Save(display);
                    RestartPiece();
                }
                else if (response == -2 && horizonalMoviment != 0 && !rotate)
                {
                    //the piece find other piece in horizontal moviment 
                    Clear(display);
                    pieceX -= horizonalMoviment;
                    Move(piece, pieceY, pieceX, display);
                }
                else if (response == -2 && rotate)
                {
                    //the piece try rotate too near other piece 
                    Clear(display);
                    if (rotate)
                    {
                        piece = Rotate(piece);
                        piece = Rotate(piece);
                        piece = Rotate(piece);
                    }
                    Move(piece, pieceY, pieceX, display);
                }
                else if (response == -1)
                {
                    //the piece find the ground
                    RestartPiece();
                    Save(display);
                }
                else if (response == -3)
                {
                    //the piece try rotate too near of border or goes above the border
                    pieceX -= horizonalMoviment;
                    Clear(display);
                    if (rotate)
                    {
                        piece = Rotate(piece);
                        piece = Rotate(piece);
                        piece = Rotate(piece);
                    }
                    Move(piece, pieceY, pieceX, display);
                }

                //reset variables for not repeat any moviment
                rotate = false;
                horizonalMoviment = 0;
                verticalMoviment = 0;

                Draw(Converter(display));
                frame++;
                Thread.Sleep(16);
            }
        }

        private static void RestartPiece()
        {
            piece = GetRandomPiece();
            pieceY = height - piece.GetLength(0);
            pieceX = random.Next(0, width - piece.GetLength(1));
        }

        private static void InitalizeTitle(int width)
        {
            for (int i = 0; i < width * 3; i++)
            {
                title += "-";
            }

            title += "\n|";
            for (int i = 0; i < (width * 3) - 2; i++)
            {
                title += " ";
            }
            title += "|\n";

            title += "|"; 
            for (int i = 0; i < ((width * 3) - 2 - 6) / 2; i++)
            {
                title += " ";
            }
            title += "Tetris";
            for (int i = 0; i < ((width * 3) - 2 - 6) / 2; i++)
            {
                title += " ";
            }
            title += "|";

            title += "\n|";
            for (int i = 0; i < (width * 3) - 2; i++)
            {
                title += " ";
            }
            title += "|\n";

            for (int i = 0; i < width * 3; i++)
            {
                title += "-";
            }

            title += "\n";
        }

        static char[,] GetRandomPiece()
        {
            Random random = new Random();
            List<char[,]> list = new List<char[,]>();
            list.Add(new char[,]
            {
                {'#','#','#','#', }
            });
            list.Add(new char[,]
            {
                { '#', ' ', ' ' },
                { '#', '#', '#' },
            });
            list.Add(new char[,]
            {
                { ' ', ' ', '#' },
                { '#', '#', '#' },
            });
            list.Add(new char[,]
            {
                { ' ', '#', ' ' },
                { '#', '#', '#' },
            });
            list.Add(new char[,]
            {
                { '#', '#' },
                { '#', '#' },
            });
            return list[random.Next(0, list.Count)];
        }

        static void Control()
        {
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                rotate = key == ConsoleKey.UpArrow;
                verticalMoviment = key == ConsoleKey.DownArrow ? -1 : verticalMoviment;
                horizonalMoviment = key == ConsoleKey.LeftArrow ? -1 : horizonalMoviment;
                horizonalMoviment = key == ConsoleKey.RightArrow ? 1 : horizonalMoviment;
            }
        }

        static char[,] Rotate(char[,] piece)
        {
            char[,] temp = new char[piece.GetLength(1), piece.GetLength(0)];
            for (int i = 0; i < piece.GetLength(0); i++)
            {
                for (int j = 0; j < piece.GetLength(1); j++)
                {
                    temp[j, i] = piece[i, piece.GetLength(1) - (j + 1)];
                }
            }
            return temp.Clone() as char[,];
        }

        static void Gravity()
        {
            while (true)
            {
                verticalMoviment--;
                Thread.Sleep(1000);
            }
        }

        static void Draw(string text)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(title);
            Console.Write(text);
        }

        static string Converter(char[,] image)
        {
            string result = "";
            string line = "";
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    line += $"[{image[i, j]}]";
                }
                result = line + "\n" + result;
                line = "";
            }
            return result;
        }

        static double ToRadians(double deg)
        {
            return (deg * Math.PI) / 180;
        }

        static void Initalize(char[,] display)
        {
            displaySaved = new char[display.GetLength(0), display.GetLength(1)];
            for (int i = 0; i < display.GetLength(0); i++)
            {
                for (int j = 0; j < display.GetLength(1); j++)
                {
                    display[i, j] = ' ';
                    displaySaved[i, j] = ' ';
                }
            }
        }

        static void Clear(char[,] display)
        {
            for (int i = 0; i < displaySaved.GetLength(0); i++)
            {
                for (int j = 0; j < displaySaved.GetLength(1); j++)
                {
                    display[i, j] = displaySaved[i, j];
                }
            }
        }

        static void Save(char[,] display)
        {
            VerifyLine(display);
            displaySaved = new char[display.GetLength(0), display.GetLength(1)];
            for (int i = 0; i < displaySaved.GetLength(0); i++)
            {
                for (int j = 0; j < displaySaved.GetLength(1); j++)
                {
                    displaySaved[i, j] = display[i, j];
                }
            }
        }

        static void VerifyLine(char[,] display)
        {
            int numberOfCollumns = display.GetLength(1);
            for (int i = 0; i < display.GetLength(0); i++)
            {
                int numberOfHashtags = 0;
                for (int j = 0; j < display.GetLength(1); j++)
                {
                    if (display[i, j] == '#')
                        numberOfHashtags++;
                }
                if(numberOfHashtags == numberOfCollumns)
                {
                    for (int l = i; l < (display.GetLength(0) - 1); l++)
                    {
                        for (int j = 0; j < display.GetLength(1); j++)
                        {
                            display[l, j] = display[l + 1, j];
                        }
                    }
                    i--;
                }
            }
        }

        static int Move(char[,] piece, int pieceY, int pieceX, char[,] display)
        {
            int response = 0;
            for (int i = 0; i < piece.GetLength(0); i++)
            {
                for (int j = 0; j < piece.GetLength(1); j++)
                {
                    int positionY = i + pieceY, positionX = j + pieceX;
                    if (positionY == 0)
                    {
                        response = -1;
                    }
                    try
                    {
                        if (display[positionY, positionX] != ' ' && piece[i, j] == '#')
                        {
                            return -2;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return -3;
                    }
                    if (display[positionY, positionX] == ' ')
                        display[positionY, positionX] = piece[i, j];
                }
            }
            return response;
        }
    }
}

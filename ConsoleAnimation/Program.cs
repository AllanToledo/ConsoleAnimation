using System;
using System.Threading;

namespace ConsoleAnimation
{
    class Program
    {
        static void Main(string[] args)
        {
            int frame = 0;
            int height = 26, width = 100;
            char[,] display = new char[height, width];



            while (true)
            {
                Clear(display);
                for (int i = 0; i < width; i++)
                {
                    int hp = (int)Math.Floor(Math.Cos(toRadians(i * frame / 100)) * 10 + (height / 2));
                    if (hp < height - 1 && hp > 0)
                    {
                        display[hp, i] = '.';
                    }
                }

                Draw(Converter(display));
                frame++;
                Thread.Sleep(5);
            }
        }

        static void Draw(string text)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(text);
        }

        static string Converter(char[,] image)
        {
            string result = "";

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    result += image[i, j];
                }
                result += "\n";
            }
            return result;
        }

        static double toRadians(double deg)
        {
            return (deg * Math.PI) / 180;
        }

        static void Clear(char[,] image)
        {

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j] = ' ';
                }
            }
        }

        static void Write(char[,] image, char letter, int offeset)
        {
            switch (letter)
            {
                case 'a':
                    char[,] letter_code =
                    {
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', '#', '#', '#', ' ', ' ' },
                        {'#', ' ', ' ', ' ', '#', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                        {' ', ' ', ' ', ' ', ' ', ' ' },
                    };
                    break;
            }
        }
    }
}

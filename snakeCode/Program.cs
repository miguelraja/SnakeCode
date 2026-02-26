using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace snakeCode
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj) =>
            obj is Position p && p.X == X && p.Y == Y;

        public override int GetHashCode() => (X, Y).GetHashCode();
    }
    public class Snake
    {
        public List<Position> Body { get; private set; } = new List<Position>();
        public Position Head => Body.Last();
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
        public string Direction { get; set; } = "RIGHT";

        public Snake(int startX, int startY)
        {
            Body.Add(new Position(startX, startY));
        }

        public void Move()
        {
            Position newHead = new Position(Head.X, Head.Y);

            switch (Direction)
            {
                case "UP": newHead.Y--; break;
                case "DOWN": newHead.Y++; break;
                case "LEFT": newHead.X--; break;
                case "RIGHT": newHead.X++; break;
            }
            Body.Add(newHead);
        }

        public void TrimTail(int score)
        {
            if (Body.Count > score)
                Body.RemoveAt(0);
        }
    }
    public class Berry
    {
        public Position Pos { get; private set; }
        private Random _random = new Random();

        public void Randomize(int width, int height)
        {
            Pos = new Position(_random.Next(1, width - 1), _random.Next(1, height - 1));
        }
    }
    public class GameEngine
    {
        private const int Width = 32;
        private const int Height = 16;
        private int _score = 5;
        private bool _gameOver = false;

        private Snake _snake = new Snake(Width / 2, Height / 2);
        private Berry _berry = new Berry();

        public void Start()
        {
            Console.WindowWidth = Width;
            Console.WindowHeight = Height;
            _berry.Randomize(Width, Height);

            while (!_gameOver)
            {
                Draw();
                Input();
                Logic();
            }
            ShowGameOver();
        }

        private void Draw()
        {
            Console.Clear();
            DrawBorders();

            Console.ForegroundColor = ConsoleColor.Green;
            DrawPixel(_berry.Pos.X, _berry.Pos.Y, "■");

            foreach (var segment in _snake.Body)
            {
                Console.ForegroundColor = (segment == _snake.Head) ? _snake.Color : ConsoleColor.White;
                DrawPixel(segment.X, segment.Y, "■");
            }
        }

        private void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < Width; i++) { DrawPixel(i, 0, "■"); DrawPixel(i, Height - 1, "■"); }
            for (int i = 0; i < Height; i++) { DrawPixel(0, i, "■"); DrawPixel(Width - 1, i, "■"); }
        }

        private void DrawPixel(int x, int y, string s)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
        }

        private void Input()
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < 500)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    _snake.Direction = key switch
                    {
                        ConsoleKey.UpArrow when _snake.Direction != "DOWN" => "UP",
                        ConsoleKey.DownArrow when _snake.Direction != "UP" => "DOWN",
                        ConsoleKey.LeftArrow when _snake.Direction != "RIGHT" => "LEFT",
                        ConsoleKey.RightArrow when _snake.Direction != "LEFT" => "RIGHT",
                        _ => _snake.Direction
                    };
                }
            }
        }

        private void Logic()
        {
            _snake.Move();

            if (_snake.Head.X <= 0 || _snake.Head.X >= Width - 1 ||
                _snake.Head.Y <= 0 || _snake.Head.Y >= Height - 1)
                _gameOver = true;

            if (_snake.Body.Take(_snake.Body.Count - 1).Any(p => p.Equals(_snake.Head)))
                _gameOver = true;

            if (_snake.Head.Equals(_berry.Pos))
            {
                _score++;
                _berry.Randomize(Width, Height);
            }

            _snake.TrimTail(_score);
        }

        private void ShowGameOver()
        {
            Console.Clear();
            Console.SetCursorPosition(Width / 5, Height / 2);
            Console.WriteLine($"Game Over! Score: {_score}");
        }
    }

    class Program
    {
        static void Main()
        {
            new GameEngine().Start();
        }
    }
}
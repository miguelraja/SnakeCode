using System;
using System.Collections.Generic;
namespace snakeCode{
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
                CheckSnakeColissions();
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
                    _snake.SnakeDirection = key switch
                    {
                        ConsoleKey.UpArrow when _snake.SnakeDirection != Directions.DOWN => Directions.UP,
                        ConsoleKey.DownArrow when _snake.SnakeDirection != Directions.UP => Directions.DOWN,
                        ConsoleKey.LeftArrow when _snake.SnakeDirection != Directions.RIGHT => Directions.LEFT,
                        ConsoleKey.RightArrow when _snake.SnakeDirection != Directions.LEFT => Directions.RIGHT,
                        _ => _snake.SnakeDirection
                    };
                }
            }
        }

        private void CheckSnakeColissions()
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

}
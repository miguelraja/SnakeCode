 using System;
using System.Collections.Generic;

namespace snakeCode{
 
 public class Snake
    {
        public List<Position> Body { get; private set; } = new List<Position>();
        public Position Head => Body.Last();
        public ConsoleColor Color { get; set; } = ConsoleColor.Red;
        public Directions SnakeDirection { get; set; } = Directions.RIGHT;

        public Snake(int startX, int startY)
        {
            Body.Add(new Position(startX, startY));
        }

        public void Move()
        {
            Position newHead = new Position(Head.X, Head.Y);

            switch (SnakeDirection)
            {
                case Directions.UP: newHead.Y--; break;
                case Directions.DOWN: newHead.Y++; break;
                case Directions.LEFT: newHead.X--; break;
                case Directions.RIGHT: newHead.X++; break;
            }
            Body.Add(newHead);
        }

        public void TrimTail(int score)
        {
            if (Body.Count > score)
                Body.RemoveAt(0);
        }
    }
}
 public class Berry
    {
        public Position Pos { get; private set; }
        private Random _random = new Random();

        public void Randomize(int width, int height)
        {
            Pos = new Position(_random.Next(1, width - 1), _random.Next(1, height - 1));
        }
    }
using System;

namespace Messages.Helpers
{
  public class NewColor : IDisposable
    {
        private readonly ConsoleColor _originalColor;

        public NewColor(ConsoleColor color)
        {
            _originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        public void Dispose()
        {
            Console.ForegroundColor = _originalColor;
        }

        public static NewColor White()
        {
            return new NewColor(ConsoleColor.White);
        }

        public static NewColor Green()
        {
            return new NewColor(ConsoleColor.Green);
        }

        public static NewColor Red()
        {
            return new NewColor(ConsoleColor.Red);
        }

        public static NewColor Yellow()
        {
            return new NewColor(ConsoleColor.Yellow);
        }

        public static NewColor Blue()
        {
            return new NewColor(ConsoleColor.Blue);
        }

        public NewColor ChangeTo(ConsoleColor newColor)
        {
            return new NewColor(newColor);
        }
    }
}

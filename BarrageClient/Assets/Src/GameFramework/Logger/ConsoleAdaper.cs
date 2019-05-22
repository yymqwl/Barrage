using System;
namespace GameFramework
{
    public class ConsoleAdaper : ALogDecorater, ILog
    {

        public ConsoleAdaper(ALogDecorater decorater = null) : base(decorater)
        {
        }

        public void Trace(string message)
        {
            WriteLine(message);
        }

        public void Warning(string message)
        {
            WriteLine(message,ConsoleColor.Yellow);
        }

        public void Info(string message)
        {
            WriteLine(message);
        }

        public void Debug(string message)
        {
            WriteLine(message);
        }

        public void Error(string message)
        {
            WriteLine(message,ConsoleColor.Red);
        }

        public void Fatal(string message)
        {
            WriteLine(message, ConsoleColor.DarkRed);
        }

        public void WriteLine(string text, ConsoleColor colour = ConsoleColor.White)
        {
            var originalColour = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ForegroundColor = originalColour;
        }

    }
}
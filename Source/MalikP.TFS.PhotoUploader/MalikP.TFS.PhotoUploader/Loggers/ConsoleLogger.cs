using MalikP.TFS.PhotoUploader.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalikP.TFS.PhotoUploader.Loggers
{
    public class ConsoleLogger : IProcessLogger, IParametrizedProcessLogger<ConsoleColor>
    {
        ConsoleColor _storedColor { get; }
        public ConsoleLogger()
        {
            _storedColor = Console.ForegroundColor;
        }

        public void Initialize(object initializationContext)
        {
        }

        public virtual void LogDebug(string message)
        {
            Log(message, ConsoleColor.Blue);
        }

        public virtual void RestoreColor()
        {
            Console.ForegroundColor = _storedColor;
        }

        public virtual void LogError(string message)
        {
            Log(message, ConsoleColor.Red);
        }

        public virtual void LogInfo(string message)
        {
            Log(message, ConsoleColor.White);
        }

        public virtual void LogWarning(string message)
        {
            Log(message, ConsoleColor.Yellow);
        }

        public virtual void Log(string message, ConsoleColor parameter)
        {
            Console.ForegroundColor = parameter;
            Console.WriteLine(message);
            RestoreColor();
        }
    }
}

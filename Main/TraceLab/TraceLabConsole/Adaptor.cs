using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceLabConsole
{
    public class Adaptor
    {
        #region Initalization

        #endregion

        public static void RunCommand(String CommandType, String CommandString)
        {
            switch (CommandType)
            {
                case "Open":
                    ConsoleUI.RunCommand(CommandString);
                    break;

                case "Run":
                    ConsoleUI.RunCommand(CommandString);
                    break;

                case "?":
                    ConsoleUI.RunCommand(CommandString);
                    break;

                default:
                    // Console.WriteLine("Command is incorrect. Display commands using ?");
                    break;
            }
        }
    }
}

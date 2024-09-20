using Figgle;

using QMNDownloader.Constants;
using QMNDownloader.Services;

using System;
using System.CommandLine;
using System.Text;
using System.Threading.Tasks;

namespace QMNDownloader
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            SetupConsole();
            DisplayHeader();

            RootCommand rootCommand = CommandFactory.CreateRootCommand();
            return await rootCommand.InvokeAsync(args);
        }

        private static void SetupConsole()
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
        }

        private static void DisplayHeader()
        {
            Console.WriteLine(FiggleFonts.Standard.Render(GeneralConstants.PROGRAM_NAME));
            Console.WriteLine($"{GeneralConstants.PROGRAM_NAME} {GeneralConstants.PROGRAM_VERSION} - (c) {GeneralConstants.PROGRAM_AUTHOR}");
            Console.WriteLine(new string('-', 32));
        }
    }
}

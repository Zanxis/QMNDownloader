using QMNDownloader.Constants;

using System.CommandLine;

namespace QMNDownloader.Services
{
    internal static class CommandFactory
    {
        public static RootCommand CreateRootCommand()
        {
            Option<string> blogUrlOption = new("--url", "The URL of the respective website blog page that will be downloaded (full URL).")
            {
                IsRequired = true,
            };

            RootCommand rootCommand = new(GeneralConstants.PROGRAM_DESCRIPTION);
            rootCommand.AddOption(blogUrlOption);
            rootCommand.SetHandler(DownloadHandler.HandleDownloadAsync, blogUrlOption);

            return rootCommand;
        }
    }
}

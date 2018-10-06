namespace FileData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            //We'd want to use a Dependency Injector in a bigger project (ninject, autofac, etc).
            var parser = new Parser();
            FileInspector fileInspector = new FileInspector(new FileDetailsAdapter());
            Program p = new Program();

            string output = p.Run(parser, fileInspector, args);

            Console.WriteLine(output);
        }

        public string Run(IParser parser, IFileInspector fileInspector, string[] args)
        {
            var options = parser.Parse(args);
            if (options.ShowUsage)
            {
                //We could be more specific about what was missing, but most command line apps just show you the general usage instructions.
                return "FileData.exe (-s | -v) filenane";
            }

            var fileInfo = fileInspector.InspectFile(options);

            //Format the resuls for the user
            var result = new List<string>();
            if (fileInfo.Size.HasValue)
            {
                result.Add($"Size: {fileInfo.Size.Value}");
            }

            if (!string.IsNullOrEmpty(fileInfo.Version))
            {
                result.Add($"Version: {fileInfo.Version}");
            }

            return result.Count() > 0 ? string.Join(", ", result.ToArray()) : "No file information";
        }
    }
}

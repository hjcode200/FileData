namespace FileData
{
    using System;

    public interface IFileInspector
    {
        FileInspectorResult InspectFile(Options options);
    }

    public class FileInspector : IFileInspector
    {
        private readonly FileDetailsAdapter fileDetails;

        public FileInspector(FileDetailsAdapter fileDetails)
        {
            this.fileDetails = fileDetails ?? throw new ArgumentException(nameof(fileDetails));
        }

        public FileInspectorResult InspectFile(Options options)
        {
            //We could do futher tests on the file name but spec says not to worry about it
            if (string.IsNullOrEmpty(options.FileName))
            {
                throw new ArgumentException(nameof(options.FileName));
            }

            return new FileInspectorResult()
            {
                Size = options.ShowSize ? (int?)fileDetails.Size(options.FileName) : null,
                Version = options.ShowVersion ? fileDetails.Version(options.FileName) : null
            };
        }
    }
}

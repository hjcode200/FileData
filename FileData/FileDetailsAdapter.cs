namespace FileData
{
    using ThirdPartyTools;

    //The FileDetails third party component is difficult to Mock out for unit tests as its creators didn't see fit to make
    //FileDetails impement an interface or mark it's methods as virtual. The only alternatives are to use Fakes / Shims / something
    //else that messes with the IL, or we can use the Adapter pattern to wrap the FileDetails component and mock the adapter.
    public class FileDetailsAdapter
    {
        private readonly FileDetails fileDetails;

        public FileDetailsAdapter()
        {
            this.fileDetails = new FileDetails();
        }

        public virtual string Version(string filePath)
        {
            return fileDetails.Version(filePath);
        }

        public virtual int Size(string filePath)
        {
            return fileDetails.Size(filePath);
        }
    }
}

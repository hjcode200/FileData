namespace FileData
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IParser
    {
        Options Parse(string[] args);
    }

    public class Parser : IParser
    {
        //The specification says they want code that:
        //Takes in two arguments (argument 1 = functionality to perform, argument 2 = filename...
        //If the second argument is anyone of –s, --s, /s, --size the return the size of the file (use FileDetails.Size)
        //This last statement contradicts "takes in two arguments". I'll take this to mean we can accept 2 or 3 arguments
        //i.e. It is valid to request both size and version with:
        //-v -s filename
        //Of course we then introduce ambiguity into the file name as file names can start with a slash (or a hyphen I think)
        //on Linux, so does:
        //-s -v
        //mean we want to view the size of the file called "-v"? I wonder if the spec is deliberately ambiguous?
        //I'll guard against file names that match known parameter names. This would be one to discuss with the Business Analyst.
        public Options Parse(string[] args)
        {
            var sizeArgumentVariants = GetArgumentVariants("size");
            var versionArgumentVariants = GetArgumentVariants("version");

            if (args.Length >= 2 && args.Length <= 3)
            {
                //Check file name doesn't match a known parameter name
                if (!sizeArgumentVariants.Contains(args.Last()) && !versionArgumentVariants.Contains(args.Last()))
                {
                    var firstActionType = GetActionType(args[0], sizeArgumentVariants, versionArgumentVariants);
                    if (firstActionType != ActionType.Unknown)
                    {
                        if (args.Length == 3)
                        {
                            var secondActionType = GetActionType(args[1], sizeArgumentVariants, versionArgumentVariants);

                            //Check the user hasn't passed the same action twice: FileData.exe -v -v filename
                            if (firstActionType != secondActionType && secondActionType != ActionType.Unknown)
                            {
                                return new Options()
                                {
                                    ShowSize = firstActionType == ActionType.Size || secondActionType == ActionType.Size,
                                    ShowVersion = firstActionType == ActionType.Version || secondActionType == ActionType.Version,
                                    FileName = args[2]
                                };
                            }
                        }
                        else
                        {
                            return new Options()
                            {
                                ShowSize = firstActionType == ActionType.Size,
                                ShowVersion = firstActionType == ActionType.Version,
                                FileName = args[1]
                            };
                        }
                    }
                }
            }

            //Invalid arguments, so show usage instructions
            return new Options() { ShowUsage = true };
        }

        private string[] GetArgumentVariants(string argumentName)
        {
            var variants = new List<string>();
            if (argumentName.Length > 0)
            {
                variants.Add("--" + argumentName);
                var firstChar = argumentName[0];
                variants.Add("-" + firstChar);
                variants.Add("--" + firstChar);
                variants.Add("/" + firstChar);
            }

            return variants.ToArray();
        }

        private enum ActionType
        {
            Unknown,
            Version,
            Size
        }

        private ActionType GetActionType(string argument, string[] sizeArgumentVariants, string[] versionArgumentVariants)
        {
            if (sizeArgumentVariants.Contains(argument))
            {
                return ActionType.Size;
            }
            else if (versionArgumentVariants.Contains(argument))
            {
                return ActionType.Version;
            }

            return ActionType.Unknown;
        }
    }
}

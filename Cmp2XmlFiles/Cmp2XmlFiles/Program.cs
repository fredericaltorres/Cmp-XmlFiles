using CommandLine;
using Org.XmlUnit;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
using System;

// https://github.com/commandlineparser/commandline
// https://github.com/xmlunit/xmlunit.net

namespace Cmd2XmlFile
{

    // compare2XMLFile --referenceXmlFile "C:\A\a.xml" --xmlTextFile "C:\A\b.xml"

    [Verb("compare2XMLFile", HelpText = "compare2xmlfile")]
    public class Compare2XmlFileCommandLineOptions
    {
        [Option('s', "referenceXmlFile", Required = false, HelpText = "reference")]
        public string ReferenceXmlFile { get; set; }


        [Option('s', "xmlTextFile", Required = false, HelpText = "xmlTextFile")]
        public string XmlTextFile { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var r = Parser.Default.ParseArguments<Compare2XmlFileCommandLineOptions>(args).MapResult(

                (Compare2XmlFileCommandLineOptions options) =>
                {
                    var diffCounter = 0;
                    Console.WriteLine($"ReferenceXmlFile: {options.ReferenceXmlFile}");
                    Console.WriteLine($"XmlTextFile: {options.XmlTextFile}");
                    ISource refFile = Input.FromFile(options.ReferenceXmlFile).Build();
                    ISource testFile = Input.FromFile(options.XmlTextFile).Build();
                    IDifferenceEngine diff = new DOMDifferenceEngine();
                    diff.DifferenceListener += (comparison, outcome) => {
                        Console.WriteLine($"Found a difference: {comparison}");
                        diffCounter += 1;
                    };
                    diff.Compare(refFile, testFile);

                    if (diffCounter == 0)
                        Console.WriteLine($"Identical {Environment.NewLine}");

                    refFile.Dispose();
                    testFile.Dispose();

                    //if (diffCounter > 0)
                    //{
                    //    try
                    //    {
                    //        System.IO.File.Delete(options.XmlTextFile);
                    //        ystem.IO.File.Copy(options.ReferenceXmlFile, options.XmlTextFile);
                    //        Console.WriteLine("File overwritten {options.XmlTextFile}");
                    //    }
                    //    catch(System.Exception ex)
                    //    {
                    //        Console.WriteLine($"Error {ex.Message}");
                    //    }
                    //}

                    Console.WriteLine("Hit space");
                    Console.ReadKey();
                    return diffCounter == 0 ? 0 : 1;
                },
                errs => 123
            );
            Environment.Exit(r);
        }
    }
}

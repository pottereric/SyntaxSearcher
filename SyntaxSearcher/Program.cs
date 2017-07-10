using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SyntaxSearcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SearchDirectoryForNodes<CasePatternSwitchLabelSyntax>(@"C:\Projects\reference");
            //SearchDirectoryForNodes<CasePatternSwitchLabelSyntax>(@"C:\Projects\PatternMatching\ShapeDemo");

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
        }

        public static void SearchDirectoryForNodes<T>(string path) where T : CSharpSyntaxNode
        {
            try
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".cs")
                    {
                        var nodes = SearchFileForNodes<T>(file);

                        if (nodes != null && nodes.Count() > 0)
                        {
                            PrintFileData(file, nodes);
                        }
                    }
                }

                var subDirs = Directory.GetDirectories(path);
                foreach (var subDir in subDirs)
                {
                    SearchDirectoryForNodes<T>(subDir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Reading Directory: {path}");
                Console.WriteLine($"Error Message: {ex.Message}");
            }
        }

        private static void PrintFileData<T>(string file, IEnumerable<T> nodes) where T : CSharpSyntaxNode
        {
            Console.WriteLine(file);

            foreach (var node in nodes)
            {
                Console.WriteLine($"Line: {node.GetLocation().GetLineSpan().StartLinePosition.Line + 1}");
            }

            Console.WriteLine();
        }

        public static IEnumerable<T> SearchFileForNodes<T>(string filePath) where T : CSharpSyntaxNode
        {
            var fileContents = File.ReadAllText(filePath);
            var ast = CSharpSyntaxTree.ParseText(fileContents).GetRoot();

            var nodes = ast.DescendantNodes().OfType<T>();

            return nodes;
        }
    }
}
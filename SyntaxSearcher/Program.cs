using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using System.Linq;

namespace SyntaxSearcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SearchDirectoryForNodes<CasePatternSwitchLabelSyntax>(@"C:\Projects\PatternMatching\ShapeDemo");

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
        }

        public static void SearchDirectoryForNodes<T>(string path) where T : CSharpSyntaxNode
        {
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".cs")
                {
                    SearchFileForNodes<T>(file);
                }
            }

            var subDirs = Directory.GetDirectories(path);
            foreach (var subDir in subDirs)
            {
                SearchDirectoryForNodes<T>(subDir);
            }
        }

        public static void SearchFileForNodes<T>(string filePath) where T : CSharpSyntaxNode
        {
            Console.WriteLine(filePath);

            var fileContents = File.ReadAllText(filePath);
            var ast = CSharpSyntaxTree.ParseText(fileContents).GetRoot();

            var patthenNodes = ast.DescendantNodes().OfType<T>();

            foreach (var whenNode in patthenNodes)
            {
                Console.WriteLine($"Line: {whenNode.GetLocation().GetLineSpan().StartLinePosition.Line + 1}");
            }

            Console.WriteLine();
        }
    }
}
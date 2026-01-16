using System;

using Ducker.Core;

namespace Ducker
{
    class Program
    {

        static void Main(string[] args)
        {
            //TODO. https://github.com/EmilPoulsen/Ducker/issues/1 
            // The idea here was to create a simple console app, instead of the
            // full blown WPF app, so that Ducker can be included in automation workflows.
            // It never got there, but please feel free to give it a go if you read this :)

            string pathToDll = @"X:\Nephila\bin\DebugR8\net8.0-windows\Nephila.Gh.gha";
            
            if (!System.IO.File.Exists(pathToDll))
            {
                Console.WriteLine($"Error: File not found at {pathToDll}");
                return;
            }

            ExportSettings settings = ExportSettings.Default;
            settings.DocWriter = settings.DocWriter ?? typeof(StandardMdDocGenerator);

            IGhaReader reader = new RhinoHeadlessGhaReader();
            IDocGenerator docGen = Activator.CreateInstance(settings.DocWriter) as IDocGenerator;
            IDocWriter docWrite = new MarkDownDocWriter();

            DuckRunner duckerRunner = new DuckRunner();
            duckerRunner.AssemblyPath = pathToDll;
            
            duckerRunner.TryInitializeRhino(reader);
            
            duckerRunner.Run(reader, docGen, docWrite);
        }
    }
}

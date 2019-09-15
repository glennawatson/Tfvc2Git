using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Tfvc2Git.Model.Services;
using Tfvc2Git.Model.Services.Model;

namespace Tfvc2Git
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    DoWork,
                    err => Task.FromResult(1));
        }

        private static Task DoWork(Options options)
        {
            var processor = new TfvcToGitProcessor(options);
            return processor.Process(default);
        }
    }
}

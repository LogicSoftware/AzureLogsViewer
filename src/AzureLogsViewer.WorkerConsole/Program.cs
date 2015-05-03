using System;
using System.Data.Entity;
using AzureLogsViewer.Model;
using AzureLogsViewer.Model.Migrations;
using AzureLogsViewer.Model.Services;
using Ninject;

namespace AzureLogsViewer.WorkerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MigrationDatabaseInitializer());

            var kernel = new StandardKernel();
            kernel.Load(new DIModule());

            var worker = kernel.Get<Worker>();

            Console.WriteLine("Starting worker");
            worker.Start();
            Console.WriteLine("Worker is started");

            Console.WriteLine("**************");
            Console.WriteLine("Press enter to stop..");
            Console.ReadLine();

            worker.Stop();

        }
    }
}

using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using AzureLogsViewer.Model;
using AzureLogsViewer.Model.Entities;
using AzureLogsViewer.Model.Infrastructure;
using AzureLogsViewer.Model.Services;
using Ninject;

namespace AzureLogsViewer.WorkerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: initialize from site ??
            Database.SetInitializer(new TempDatabaseInitializer());

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

    //TODO: remove it..
    internal class TempDatabaseInitializer : DropCreateDatabaseIfModelChanges<AlwDataContext>
    {
        protected override void Seed(AlwDataContext context)
        {
            base.Seed(context);

            context.WadLogsDumpSettings.AddOrUpdate(x => x.Id, new WadLogsDumpSettings { Id = 1 });
        }
    }
}

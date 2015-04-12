using System.Data.Entity;
using System.ServiceProcess;
using AzureLogsViewer.Model;
using AzureLogsViewer.WorkerConsole;
using Ninject;

namespace AzureLogsViewer.Worker
{
    public partial class AzureLogsViewerService : ServiceBase
    {
        private Model.Services.Worker _worker;

        public AzureLogsViewerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //TODO: initialize from site ??
            Database.SetInitializer(new TempDatabaseInitializer());

            var kernel = new StandardKernel();
            kernel.Load(new DIModule());

            _worker = kernel.Get<Model.Services.Worker>();
            _worker.Start();
        }

        protected override void OnStop()
        {
            _worker.Stop();
        }
    }
}

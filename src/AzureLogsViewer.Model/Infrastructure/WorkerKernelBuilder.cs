using Ninject;
using Ninject.Extensions.ChildKernel;

namespace AzureLogsViewer.Model.Infrastructure
{
    internal class WorkerKernelBuilder
    {
        public static IKernel Create(IKernel kernel)
        {
            var childKernel = new ChildKernel(kernel);
            //use single data context for worker operation and dispose it immediately after operation was completed.
            childKernel.Rebind<AlwDataContext>().To<AlwDataContext>().InSingletonScope();

            return childKernel;
        }
    }
}
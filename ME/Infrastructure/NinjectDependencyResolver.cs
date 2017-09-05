using System;
using System.Collections.Generic;
using Ninject;
using System.Web;
using HRRcp.Areas.ME.Models;
using HRRcp.Areas.ME.Models.CustomModels;
using HRRcp.Areas.ME.Models.Interfaces;
using System.Web.Mvc;

namespace HRRcp.Areas.ME.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IMenuSet>().To<MainMenuMatryca>();
        }
    }
}
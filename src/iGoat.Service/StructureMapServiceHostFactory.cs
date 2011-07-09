using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using StructureMap;

namespace iGoat.Service
{
    public class StructureMapServiceHostFactory : ServiceHostFactory
    {
        public StructureMapServiceHostFactory()
        {
            var container = ObjectFactory.Container;
            new Bootstrapper(container).Run();
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new StructureMapServiceHost(serviceType, baseAddresses);
        }
    }
}
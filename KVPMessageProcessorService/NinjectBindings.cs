using KVP_Obrazci.Domain.Abstract;
using KVP_Obrazci.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessorService
{
    class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEmployeeRepository>().To<EmployeeRepository>();
        }
    }
}

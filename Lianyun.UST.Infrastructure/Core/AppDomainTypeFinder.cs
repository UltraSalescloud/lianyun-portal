using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Lianyun.UST.Infrastructure.Core
{
    public class AppDomainTypeFinder : ITypeFinder
    {
        private readonly IAssemblyTypeFinder _assemblyTypeFinder;

        public AppDomainTypeFinder(IAssemblyTypeFinder assemblyTypeFinder)
        {
            this._assemblyTypeFinder = assemblyTypeFinder;
        }

        protected virtual IEnumerable<Assembly> GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcrete = true)
        {
            var assemblies = this.GetAssemblies();

            return this._assemblyTypeFinder.FindClassesOfType(type, assemblies, onlyConcrete);
        }

        public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcrete = true)
        {
            return this.FindClassesOfType(typeof(T), onlyConcrete);
        }
    }
}
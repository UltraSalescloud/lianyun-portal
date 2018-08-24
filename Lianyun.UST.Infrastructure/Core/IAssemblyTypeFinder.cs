using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lianyun.UST.Infrastructure.Core
{
    public interface IAssemblyTypeFinder
    {
        IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcrete = true);

        IEnumerable<Type> FindClassesOfType(Type type, IEnumerable<Assembly> assemblies, bool onlyConcrete = true);
    }
}
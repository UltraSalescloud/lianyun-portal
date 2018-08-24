using System;
using System.Collections.Generic;

namespace Lianyun.UST.Infrastructure.Core
{
    public interface ITypeFinder
    {
        IEnumerable<Type> FindClassesOfType(Type type, bool onlyConcrete = true);

        IEnumerable<Type> FindClassesOfType<T>(bool onlyConcrete = true);
    }
}
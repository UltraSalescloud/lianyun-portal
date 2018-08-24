using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lianyun.UST.Infrastructure.Core
{
    public class AssemblyTypeFinder : IAssemblyTypeFinder
    {
        public IEnumerable<Type> FindClassesOfType<T>(IEnumerable<Assembly> assemblies, bool onlyConcrete = true)
        {
            return this.FindClassesOfType(typeof(T), assemblies, onlyConcrete);
        }

        public IEnumerable<Type> FindClassesOfType(Type type, IEnumerable<Assembly> assemblies, bool onlyConcrete = true)
        {
            var result = new List<Type>();

            foreach (var a in assemblies)
            {
                foreach (var t in a.GetTypes())
                {
                    if (type.IsAssignableFrom(t) || (type.IsGenericTypeDefinition && DoesTypeImplementOpenGeneric(t, type)))
                    {
                        if (!t.IsInterface)
                        {
                            if (onlyConcrete)
                            {
                                if (t.IsClass && !t.IsAbstract)
                                    result.Add(t);
                            }
                            else
                            {
                                result.Add(t);
                            }
                        }
                    }
                }
            }

            return result;
        }

        protected virtual bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();

            foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
            {
                if (implementedInterface.IsGenericType)
                {
                    var isMatch = genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());

                    if (isMatch)
                        return true;
                }
            }
            return false;
        }
    }
}
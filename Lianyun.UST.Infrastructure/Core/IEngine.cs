using System;

namespace Lianyun.UST.Infrastructure.Core
{
    public interface IEngine
    {
        T Resolve<T>();

        T ResolveNamed<T>(string name);

        T ResolveKeyed<T, TKey>(TKey key);

        T ResolveOptional<T>() where T : class;

        T ResolveOptionalNamed<T>(string name) where T : class;

        T ResolveOptionalKeyed<T, TKey>(TKey key) where T: class;

        object Resolve(Type targetType);

        object ResolveNamed(Type targetType, string name);

        object ResolveOptional(Type targetType);     
    }
}

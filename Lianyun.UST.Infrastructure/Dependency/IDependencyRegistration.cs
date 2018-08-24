using Autofac;

namespace Lianyun.UST.Infrastructure.Dependency
{
    public interface IDependencyRegistration
    {
        void Regist(ContainerBuilder builder);

        int Order { get; }
    }
}

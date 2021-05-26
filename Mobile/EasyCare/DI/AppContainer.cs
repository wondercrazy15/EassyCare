using Autofac;

namespace EasyCare.DI
{
    public class AppContainer
    {
        public static IContainer Container { get; set; }

        public static IType Resolve<IType>()
        {
            return Container.Resolve<IType>();
        }
    }
}
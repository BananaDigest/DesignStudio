using Autofac;
using DesignStudio.Composition;


namespace DesignStudio.UI
{
    internal class Program
    {
        static void Main()
        {
            // Вuild Autofac Container 
            var builder = new ContainerBuilder();
            // Register all dependencies via module
            builder.RegisterModule<DependencyInjection>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var menuManager = scope.Resolve<MenuManager>();
                menuManager.Run();
            }
        }
    }
}

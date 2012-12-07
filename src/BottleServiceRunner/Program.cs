using FubuCore;
using Topshelf;

namespace BottleServiceRunner
{
    internal static class Program
    {
        public static void Main(params string[] args)
        {
            var application = new TopshelfApplication();
            var runner = application.Bootstrap();

            var directory = TopshelfPackageFacility.GetApplicationDirectory().ToFullPath();
            var settings = new FileSystem().LoadFromFile<BottleServiceConfiguration>(directory, BottleServiceConfiguration.FILE);

            HostFactory.Run(x =>
            {
               x.SetServiceName(settings.Name);
               x.SetDisplayName(settings.DisplayName);
               x.SetDescription(settings.Description);

               x.RunAsLocalService();

               x.Service<Bottles.Services.BottleServiceRunner>(s =>
               {
                    s.ConstructUsing(name => runner);
                    s.WhenStarted(r => r.Start());
                    s.WhenStopped(r => r.Stop());
                    s.WhenPaused(r => r.Stop());
                    s.WhenContinued(r => r.Start());
                    s.WhenShutdown(r => r.Stop());
               });

               x.StartAutomatically();
            });
        }
    }
}
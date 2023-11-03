namespace AdCommunity.Core.CustomMediator
{
    public class YtServiceProvider
    {
        private static IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider => _serviceProvider;

        public static void SetInstance(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
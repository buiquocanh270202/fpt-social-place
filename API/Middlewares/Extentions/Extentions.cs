
namespace API.Middlewares.Extentions
{
    public static class Extentions
    {
        public static void ExtentionServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var extention = typeof(Program).Assembly.ExportedTypes.Where(x => typeof(IExtentions).IsAssignableFrom(x)
                                                                            && !x.IsAbstract
                                                                            && !x.IsInterface)
                                                                    .Select(Activator.CreateInstance)
                                                                    .Cast<IExtentions>().ToList();
            extention.ForEach(extention => extention.ExtentionServices(services, configuration));

        }

    }

}

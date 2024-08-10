//using FluentValidation;
//using Microsoft.Extensions.DependencyInjection;
//using System.Reflection;

//namespace BuildingBlocks;

//public static class CommonDI
//{
//    public static IServiceCollection DIValidatorAndLogging(this IServiceCollection services)
//    {
//        services.AddMediatR(config =>
//        {
//            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
//            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
//            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
//        });
//        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//        return services;
//    }
//}

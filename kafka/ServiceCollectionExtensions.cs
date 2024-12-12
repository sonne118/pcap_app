using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serialization;

namespace kafka
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaPub(this IServiceCollection services, string topic)
        {
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddSingleton<IKafkaMessagePub, KafkaMessagePub>();
            return services;
        }

        public static IServiceCollection AddKafkaSub(this IServiceCollection services, string topic)
        {

            services.AddSingleton<IHostedService>(sp =>
               new KafkaConsumer(sp.GetRequiredService<IConfiguration>(),
                   sp.GetRequiredService<ILogger<KafkaConsumer>>(),
                   sp.GetRequiredService<ISerializer>(),
                   sp.GetRequiredService<IEnumerable<IMessageHandler>>(),
                   topic));
            return services;
        }
    }
}

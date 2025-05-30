using Microsoft.Extensions.DependencyInjection;


namespace kafka
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddKafkaPublish(this IServiceCollection services, string topic)
        {
            services.AddSingleton<ISerializer, Serializer>();
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddSingleton<IKafkaMessagePub, KafkaMessagePub>();
            return services;
        }
    }
}

using System.Threading.Tasks;

namespace kafka;
public interface ITopicRepository 
{
    Task TryCreateTopic(string topicName);
}
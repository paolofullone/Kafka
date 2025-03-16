using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace KafkaPlayground.Settings
{
    public class TopicSettings
    {
        public TopicName KafkaPlaygroundPublisher { get; set; }
    }
}

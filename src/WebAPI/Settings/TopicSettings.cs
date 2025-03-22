using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Settings
{
    public class TopicSettings
    {
        public TopicName KafkaPlaygroundPublisher { get; set; }
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RouteWise.Bot.Models;

public class Notification
{
    [JsonPropertyName("eventId")]
    public string EventId { get; set; }

    [JsonPropertyName("eventMs")]
    public long? EventMs { get; set; }

    [JsonPropertyName("eventTime")]
    public DateTime? EventTime {  get; set; }

    [JsonPropertyName("eventType")]
    public string EventType { get; set; }

    [JsonPropertyName("orgId")]
    public long OrgId { get; set; }

    [JsonPropertyName("webhookId")]
    public string WebhookId { get; set; }

    [JsonPropertyName("event")]
    public JsonElement Event { get; set; }

    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
}

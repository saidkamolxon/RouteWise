using System.Text.Json.Serialization;

namespace RouteWise.Bot.Enums;

public enum EWebhookV1AlertCondition
{
    [JsonPropertyName("DeviceSevereSpeedAboveSpeedLimit")]
    DeviceSevereSpeedAboveSpeedLimit
}
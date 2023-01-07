using SolarniBaron.Domain.Contracts;

namespace SolarniBaron.Domain.BatteryBox;

public record ApiClientResponse(int ResponseCode, ResponseStatus Status, string Message)
{
    /*public static OigApiResponse FromJson(string json)
    {
        var node= JsonSerializer.Deserialize<JsonNode>(json);
    }*/
}

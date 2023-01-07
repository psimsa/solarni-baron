namespace SolarniBaron.Domain.BatteryBox.Models.Fve;

public record FveResult
(
    Dictionary<string, FveObject> FveObjects
);
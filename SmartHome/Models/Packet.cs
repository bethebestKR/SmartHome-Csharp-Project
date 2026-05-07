using System.Text.Json.Serialization;

namespace SmartHome.Models
{
    /// <summary>
    /// WPF ↔ Unity 간 WebSocket JSON 패킷 구조
    /// </summary>
    public class Packet
    {
        [JsonPropertyName("room")]
        public string Room { get; set; } = "";       // entrance, living, kitchen, bedroom, bathroom

        [JsonPropertyName("device")]
        public string Device { get; set; } = "";     // light, security, door, fan, ...

        [JsonPropertyName("action")]
        public string Action { get; set; } = "update";

        [JsonPropertyName("payload")]
        public Dictionary<string, object> Payload { get; set; } = new();

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
    }

    /// <summary>
    /// 로그 항목
    /// </summary>
    public class LogEntry
    {
        public string Time    { get; set; } = DateTime.Now.ToString("HH:mm:ss");
        public string Room    { get; set; } = "";
        public string Message { get; set; } = "";
        public LogType Type   { get; set; } = LogType.System;
    }

    public enum LogType { Danger, Auto, Manual, System }
}

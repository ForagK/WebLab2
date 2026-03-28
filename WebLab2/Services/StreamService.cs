using Azure;
using System.Text.Json;
using WebLab2.Interfaces;

namespace WebLab2.Services
{
    public class StreamService: IStreamService
    {
        private List<String> _levels = new List<String> { "info_log", "error_log" };
        private List<String> _messages = new List<String> { "Message 1", "Message 2", "Message 3", "Message 4", "Message 5" };

        public int CurrentId { get {  return _currentId; } }
        private int _currentId = 0;
        private readonly List<string> _oldLogs = new();

        public async Task<String> GetLog()
        {
            _currentId++;

            var level = _levels[Random.Shared.Next(0, _levels.Count)];
            var message = _messages[Random.Shared.Next(0, _messages.Count)];

            var logEntry = new { msg = message };
            string json = JsonSerializer.Serialize(logEntry);

            string response = $"id: {_currentId}\nevent: {level}\ndata: {json}\n\n";

            _oldLogs.Add(response);

            return response;
        }

        public List<string> GetOldLogs(int startFrom)
        {
            Console.WriteLine("old log with " + startFrom);
            return _oldLogs.Skip(startFrom - 1).ToList();
        }
    }
}

namespace WebLab2.Interfaces
{
    public interface IStreamService
    {
        public int CurrentId { get; }
        public Task<String> GetLog();
        public List<string> GetOldLogs(int startFrom);
    }
}

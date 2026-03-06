namespace WebLab2.Models
{
    public class StreamParams
    {
        public int AfterId { get; set; } = 0;
        private int _limit = 10;
        public int Limit
        {
            get => _limit;
            set => _limit = (value > 50) ? 50 : value;
        }
    }
}

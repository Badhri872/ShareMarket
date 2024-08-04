namespace Services.WebSocket
{
    public class HistoryDataParams
    {
        public string Exchange;

        public int InstrumentToken;

        /// <summary>
        /// Intervals displayed in the graph.
        /// </summary>
        public string Interval;

        /// <summary>
        /// Timestamp in Unix format seconds.
        /// </summary>
        public long From;

        /// <summary>
        /// Timestamp in Unix format seconds.
        /// </summary>
        public long To;

        public bool Index { get; set; }
    }
}

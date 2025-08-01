namespace PropVivo.Application.Dto.TaskQuery
{
    public class QueryStatsResponse
    {
        public int TotalQueries { get; set; }
        public int OpenQueries { get; set; }
        public int InProgressQueries { get; set; }
        public int ResolvedQueries { get; set; }
        public int ClosedQueries { get; set; }
        public decimal AverageResolutionTimeInHours { get; set; }
    }
}
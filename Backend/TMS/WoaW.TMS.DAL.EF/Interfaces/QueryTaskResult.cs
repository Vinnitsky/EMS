namespace WoaW.TMS.Model.DAL
{
    public class QueryTaskResult
    {
        public QueryTaskRequest Request { get; set; }
        public ITask[] Items { get; set; }

        public long TotalCount { get; set; }
    }
}
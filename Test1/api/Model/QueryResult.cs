using System.Collections.Generic;

namespace Test1.Api.Model
{
    public class QueryResult<T>
    {
        public IEnumerable<T> Records { get; set; }
        public int? Total { get; set; }
        public T Record { get; set; }
    }
}

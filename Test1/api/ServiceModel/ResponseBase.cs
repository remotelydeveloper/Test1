using System.Collections.Generic;
using ServiceStack;

namespace Test1.Api.ServiceModel
{
    public class ResponseBase<T>
    {
        public ResponseStatus ResponseStatus { get; set; }
        public T Result { get; set; }
        public IEnumerable<T> Results { get; set; }
        public int? Total { get; set; }
    }
}
using ServiceStack;
using Test1.Api.ServiceModel;
using System.Linq;
using System.Net;

namespace Test1.Api.ServiceInterface
{
    public class BaseService : Service
    {
        protected HttpResult ErrorResult(string[] errors)
        {
            return new HttpResult(new ResponseBase<object>()
            {
                ResponseStatus = new ResponseStatus()
                {
                    Errors = errors.Select(e => new ResponseError() { Message = e }).ToList()
                }
            }, HttpStatusCode.InternalServerError);
        }
    }
}
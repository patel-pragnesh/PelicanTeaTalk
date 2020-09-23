using System.Collections.Generic;
using System.Linq;

namespace WPSTORE.Models.Authentication.Base
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }
        public BaseResponse(IEnumerable<Error> errors)
        {
            Errors = errors;
        }
        public IEnumerable<Error> Errors { get; set; }
        public bool Success => Errors == null || !Errors.Any();
        public T Content { get; set; }
    }
}

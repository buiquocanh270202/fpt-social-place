using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CQRS.Query
{
    public interface IQuery<TResponse> :IRequest<Result<TResponse>>
    {
    }
    public interface IBaseQuery
    {

    }
}

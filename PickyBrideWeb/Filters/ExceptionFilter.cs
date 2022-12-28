using DataContracts;
using HostedServiceAndDI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PickyBrideWeb.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is UnviewedContenderException exception)
        {
            context.Result = new ConflictObjectResult(new ErrorMessage(exception.Message));
        }
    }
}
using HomeHelper.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeHelper.UI.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is IBaseRequest);

            if (param.Value == null)
            {
                context.Result = new ViewResult();
                //context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new ViewResult();
                return;
                //context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}

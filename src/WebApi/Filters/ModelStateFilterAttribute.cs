using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lattice.WebApi.Filters;

/// <summary>
///  A filter to be applied to specific routes that need the model state to be
///  valid. If ModelState is invalid it returns a reponse code of 400 (BadRequest)
///  with a ErrorMessage object.
/// </summary>
public class ModelStateFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var modelState = context.ModelState;

        if (!modelState.IsValid)
            context.Result = new BadRequestObjectResult(new ErrorMessage("Model state is invalid"));
    }
}

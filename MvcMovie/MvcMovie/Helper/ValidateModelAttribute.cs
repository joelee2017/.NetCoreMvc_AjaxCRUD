using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Linq;

namespace MvcMovie.Helper
{
    public class ValidateModelAttribute: ActionFilterAttribute
    {
        /// <summary> 動作：Model 驗證 </summary>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                object errors = new
                {
                    // ModelState錯誤訊息
                    ModelStateErrors = actionContext.ModelState.Where(x => x.Value.Errors.Count > 0)
                                      .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray()),
                };

                // 回傳
                ContentResult content = new ContentResult();
                content.ContentType = "application/json";
                content.Content = JsonConvert.SerializeObject(errors);
                actionContext.Result = content;
                base.OnActionExecuting(actionContext);
            }


        }
    }
}

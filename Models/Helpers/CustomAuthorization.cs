using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helpers {
    public class CustomAuthorization : AuthorizeAttribute {
        public void OnAuthorization(AuthorizationFilterContext context) {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated) {
                context.Result = new UnauthorizedResult();
                return;
            }

            var roles = Roles.Split(',');
            if (roles.Any() && !roles.Any(role => user.IsInRole(role))) {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}

namespace D2.Authentication

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters

type SecurityHeadersAttribute () =
    inherit ActionFilterAttribute ()

    override this.OnResultExecuting (context : ResultExecutingContext) =
        let result = context.Result :?> ViewResult
        if result <> null then
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            if not (context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options")) then
                context.HttpContext.Response.Headers.Add("X-Content-Type-Options", !> "nosniff")

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            if not (context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options")) then
                context.HttpContext.Response.Headers.Add("X-Frame-Options", !> "SAMEORIGIN")

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            let csp = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self';"
            // also consider adding upgrade-insecure-requests once you have HTTPS in place for production
            //csp += "upgrade-insecure-requests;";
            // also an example if you need client images to be displayed from twitter
            // csp += "img-src 'self' https://pbs.twimg.com;";

            // once for standards compliant browsers
            if not (context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy")) then
                context.HttpContext.Response.Headers.Add("Content-Security-Policy", !> csp)
            // and once again for IE
            if not (context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy")) then
                context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", !> csp)

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            let referrer_policy = "no-referrer"
            if not (context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy")) then
                context.HttpContext.Response.Headers.Add("Referrer-Policy", !> referrer_policy);
        ()


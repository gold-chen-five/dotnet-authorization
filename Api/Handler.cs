namespace Api
{
    public static class Handler
    {
        public static async void HandleResponseJson(HttpContext ctx, int code, object rsp)
        {
            ctx.Response.StatusCode = code;
            await ctx.Response.WriteAsJsonAsync(rsp);
        }
        public static async void HandleResponseString(HttpContext ctx, int code, string rsp)
        {
            ctx.Response.StatusCode = code;
            await ctx.Response.WriteAsync(rsp);
        }

        public static async void HandleErrorResponse(HttpContext ctx, int code, string errorMsg)
        {
            ctx.Response.StatusCode = code;
            var rsp = new { error = errorMsg };
            await ctx.Response.WriteAsJsonAsync(rsp);
        }

    }
}
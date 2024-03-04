using FluentValidation;

namespace API.Utils;

public static class ValidatorExtension
{
    public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder builder) where T : class
    {
        builder.Add(eb =>
        {
            var request = eb.RequestDelegate;
            eb.RequestDelegate = async context =>
            {
                var validator = context.RequestServices.GetRequiredService<IValidator<T>>();
                context.Request.EnableBuffering();
                var body = await context.Request.ReadFromJsonAsync<T>();
                if (body is null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid body");
                    return;
                }
                var result = await validator.ValidateAsync(body);
                if (!result.IsValid)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(result.Errors);
                    return;
                }
                context.Request.Body.Position = 0;
                await request!(context);
            };
        });
        return builder;
    }
}
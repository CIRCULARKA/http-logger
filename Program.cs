var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/all", async (ctx) => 
{
    await WriteResponseAsync(ctx, "<h1>Headers</h1>");
    await WriteResponseAsync(ctx, GetHeaders(ctx));
    await WriteResponseAsync(ctx, "<h1>Body</h1>");
    await WriteResponseAsync(ctx, await GetBodyAsync(ctx));
});

app.MapGet("/headers", async (ctx) => 
{
    await WriteResponseAsync(ctx, "<h1>Headers</h1>");
    await WriteResponseAsync(ctx, GetHeaders(ctx));
});

app.MapGet("/body", async (ctx) => 
{
    await WriteResponseAsync(ctx, "<h1>Body</h1>");
    await WriteResponseAsync(ctx, await GetBodyAsync(ctx));
});

string GetHeaders(HttpContext context)
{
    if (context.Request.Headers.Any() is false)
        return "No headers :(";

    var resultBuilder = new StringBuilder();
    foreach (var pair in context.Request.Headers)
        resultBuilder.AppendLine($"{pair.Key}: \"{string.Join(", ", pair.Value)}\"<br />");

    return resultBuilder.ToString();
}

async Task<string> GetBodyAsync(HttpContext context)
{
    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();

    if (string.IsNullOrWhiteSpace(body))
        body = "No body :(";

    return body;
}

async Task WriteResponseAsync(HttpContext ctx, string content) =>
    await ctx.Response.WriteAsync(content);

app.Run();

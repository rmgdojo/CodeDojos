using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi(options => options.AddScalarTransformers());

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options
            .WithTitle("Chess Board API Documentation")
            .WithClassicLayout()
            .HideSearch()
            .ShowOperationId()
            .ExpandAllTags()
            .SortTagsAlphabetically()
            .SortOperationsByMethod()
            .PreserveSchemaPropertyOrder();
    });
}

app.Run();
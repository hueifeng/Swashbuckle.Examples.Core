using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Examples.Attributes;
using System;

namespace Swashbuckle.Examples
{
    public interface IExamplesProvider
    {
        void Initializer(OpenApiDocument swaggerDoc,
            DocumentFilterContext context);

        (Type type, string path) GetExamples(SwaggerRequestExampleAttribute attribute, OpenApiDocument swaggerDoc,
            DocumentFilterContext context, ApiDescription apiDescription);

        void EndHandler(OpenApiDocument swaggerDoc, DocumentFilterContext context);
    }
}
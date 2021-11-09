using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Examples.Attributes;
using System;

namespace Swashbuckle.Examples
{
    public class ExamplesProvider : IExamplesProvider
    {
        public (Type type, string path) GetExamples(SwaggerRequestExampleAttribute attribute, OpenApiDocument swaggerDoc,
            DocumentFilterContext context, ApiDescription apiDescription)
        {
            if (attribute == null)
            {
                return (null, null);
            }

            Type type = attribute.RequestType;
            return (type, $"/{apiDescription.RelativePath}");
        }
    }
}

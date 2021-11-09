using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Examples.Attributes;
using System;
using System.Linq;

namespace Swashbuckle.Examples
{
    public class ExamplesProvider : IExamplesProvider
    {
        public (Type type, string path) GetExamples(SwaggerRequestExampleAttribute attribute, OpenApiDocument swaggerDoc,
            DocumentFilterContext context, ApiDescription apiDescription)
        {
            var p = apiDescription.ParameterDescriptions.FirstOrDefault();
            if (attribute == null)
            {
                return (p?.Type, $"/{apiDescription.RelativePath}");
            }

            Type type = attribute.RequestType;
            return (type, $"/{apiDescription.RelativePath}");
        }
    }
}

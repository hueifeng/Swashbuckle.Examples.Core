using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.Examples.Attributes;
using System.Linq;
using System.Collections;

namespace Swashbuckle.Examples
{
    public class ExamplesDocFilter : IDocumentFilter
    {
        private readonly IExamplesProvider _examplesProvider;

        public ExamplesDocFilter(IExamplesProvider examplesProvider)
        {
            this._examplesProvider = examplesProvider;
        }



        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //var paths = new OpenApiPaths();
            foreach (var apiDescription in context.ApiDescriptions)
            {
                apiDescription.TryGetMethodInfo(out var methodInfo);
                var attribute = methodInfo.GetCustomAttribute<SwaggerRequestExampleAttribute>();
                (Type type, string path) =
                    _examplesProvider.GetExamples(attribute, swaggerDoc, context, apiDescription);

                if (type != null)
                {
                    var val = swaggerDoc.Paths[path];
                    if (val.Operations.ContainsKey(OperationType.Post))
                    {
                        var apiOperation = val.Operations[OperationType.Post];
                        swaggerDoc.Paths.Remove(path);
                        swaggerDoc.Paths.Add(path, new OpenApiPathItem
                        {
                            Operations = new Dictionary<OperationType, OpenApiOperation>
                            {
                                [OperationType.Post] = new OpenApiOperation
                                {
                                    Tags = new[]
                                    {
                                    new OpenApiTag
                                    {
                                        Name = (apiDescription.ActionDescriptor as ControllerActionDescriptor)
                                            ?.ControllerName
                                    }
                                },
                                    RequestBody = new OpenApiRequestBody
                                    {
                                        Content = new Dictionary<string, OpenApiMediaType>
                                        {
                                            ["application/json"] = new OpenApiMediaType
                                            {
                                                Schema = context.SchemaGenerator.GenerateSchema(type,
                                                    context.SchemaRepository)
                                            }
                                        }
                                    },
                                    Summary = apiOperation.Summary
                                }
                            }
                        });
                    }
                }
            }
            //swaggerDoc.Paths = paths;
        }

    }
}

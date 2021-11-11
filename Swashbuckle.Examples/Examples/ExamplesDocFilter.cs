using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Examples.Attributes;

namespace Swashbuckle.Examples
{
    public class ExamplesDocFilter : IDocumentFilter
    {
        private readonly IExamplesProvider _examplesProvider;

        public ExamplesDocFilter(IExamplesProvider examplesProvider)
        {
            _examplesProvider = examplesProvider;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            _examplesProvider.Initializer(swaggerDoc, context);
            foreach (var apiDescription in context.ApiDescriptions)
            {
                apiDescription.TryGetMethodInfo(out var methodInfo);
                var attribute = methodInfo.GetCustomAttribute<SwaggerRequestExampleAttribute>();
                (Type type, string path) =
                    _examplesProvider.GetExamples(attribute, swaggerDoc, context, apiDescription);
                if (GetControllerAndActionAttributes<SwaggerHiddenExampleAttribute>(apiDescription)
                    .OfType<SwaggerHiddenExampleAttribute>()
                    .Any())
                {
                    swaggerDoc.Paths.Remove(path);
                    continue;
                }

                if (type != null)
                {
                    var val = swaggerDoc.Paths[path];
                    if (val.Operations.ContainsKey(OperationType.Post))
                    {
                        var apiOperation = val.Operations[OperationType.Post];
                        var response = val.Operations[OperationType.Post].Responses;
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
                                    Responses = response,
                                    Summary = apiOperation.Summary,
                                }
                            }
                        });
                    }
                }
            }

            _examplesProvider.EndHandler(swaggerDoc, context);
        }


        private static IEnumerable<T> GetControllerAndActionAttributes<T>(
            ApiDescription apiDescription)
            where T : Attribute
        {
            apiDescription.TryGetMethodInfo(out var methodInfo);
            var customAttributes1 = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
            var customAttributes2 = methodInfo.GetCustomAttributes<T>();
            var objList = new List<T>(customAttributes1);
            objList.AddRange(customAttributes2);
            return objList;
        }
    }
}
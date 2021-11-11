using System;

namespace Swashbuckle.Examples.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SwaggerHiddenExampleAttribute : Attribute
    {

    }
}

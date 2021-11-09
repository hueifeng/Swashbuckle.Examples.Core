using System;

namespace Swashbuckle.Examples.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerRequestExampleAttribute : Attribute
    {
        public SwaggerRequestExampleAttribute(Type requestType)
        {
            RequestType = requestType;
        }

        public Type RequestType { get; }

    }
}

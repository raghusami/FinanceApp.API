using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceApp.API.Swagger
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
            operation.Deprecated |= apiDescription.IsDeprecated();

            foreach (var responseType in apiDescription.SupportedResponseTypes)
            {
                var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
                var response = operation.Responses[responseKey];

                foreach (var contentType in response.Content.Keys)
                {
                    if (!responseType.ApiResponseFormats.Any(x => x.MediaType == contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }

            if (operation.Parameters == null) return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                parameter.Description ??= description.ModelMetadata?.Description;
                parameter.Required |= description.IsRequired;
            }
        }
    }
}

using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;
using System.Net;
using System.Threading.Tasks;

namespace ApiClientGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var petStoreApiClient = await GenerateClientCodeFromOpenApiSpecs(
                specsUrl: "https://petstore.swagger.io/v2/swagger.json",
                clientName: "PetStoreApiClient",
                @namespace: "PetStore.ApiClient",
                generateExceptionClasses: true);

            ;
        }

        private static async Task<string> GenerateClientCodeFromOpenApiSpecs(string specsUrl, string clientName, string @namespace, bool generateExceptionClasses = false)
        {
            OpenApiDocument doc;

            using (var webClient = new WebClient())
            {
                doc = await OpenApiDocument.FromJsonAsync(webClient.DownloadString(specsUrl));
            }

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = clientName,
                InjectHttpClient = true,
                UseBaseUrl = false,
                GenerateClientInterfaces = true,
                GenerateExceptionClasses = generateExceptionClasses,
                CSharpGeneratorSettings =
                {
                    Namespace = @namespace,
                    DateTimeType = "System.DateTime",
                    DateType = "System.DateTime",
                    GenerateNullableReferenceTypes = false,
                }
            };

            var generator = new CSharpClientGenerator(doc, settings);

            return generator.GenerateFile();
        }
    }
}

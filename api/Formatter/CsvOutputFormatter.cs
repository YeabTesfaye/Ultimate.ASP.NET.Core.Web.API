using Microsoft.Net.Http.Headers;  
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shared.DataTransferObjects;

namespace api.Formatter
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv")); // Ensure this uses the correct namespace
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            return typeof(CompanyDto).IsAssignableFrom(type) ||
                   typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();

            if (context.Object is IEnumerable<CompanyDto> companies)
            {
                foreach (var company in companies)
                {
                    FormatCsv(buffer, company);
                }
            }
            else if (context.Object is CompanyDto company)
            {
                FormatCsv(buffer, company);
            }

            // Write the CSV content to the response using the selected encoding
            await response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private static void FormatCsv(StringBuilder buffer, CompanyDto company)
        {
            // Correct handling of commas and quotes within fields
            var name = company.Name?.Replace("\"", "\"\"") ?? string.Empty;
            var address = company.FullAddress?.Replace("\"", "\"\"") ?? string.Empty;

            buffer.AppendLine($"{company.Id},\"{name}\",\"{address}\"");
        }
    }
}

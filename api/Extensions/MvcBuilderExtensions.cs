using api.Formatter;
using Microsoft.Extensions.DependencyInjection;

namespace api.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder){
        builder.AddMvcOptions(options => {
            options.OutputFormatters.Add(new CsvOutputFormatter());
        });
        return builder; 
    }
}
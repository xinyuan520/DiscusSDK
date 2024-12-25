using Discus.SDK.Nacos.Configurations;
using Discus.SDK.Nacos.Extensions;
using Discus.Shared.Webapi;
using Discus.Shared.WebApi.Extensions;
using Microsoft.Extensions.Configuration;
using Nacos.AspNetCore.V2;
using Nacos.V2.DependencyInjection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Extensions.Hosting;

public static class WebApplicationBuilderExtension
{
    /// <summary>
    /// Configure Configuration/ServiceCollection/Logging
    /// <param name="builder"></param>
    /// <param name="serviceInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static WebApplicationBuilder ConfigureDefault(this WebApplicationBuilder builder, IServiceInfo serviceInfo)
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));
        if (serviceInfo is null)
            throw new ArgumentNullException(nameof(serviceInfo));

        // Configuration
        var initialData = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ServiceName", serviceInfo.ServiceName) };
        builder.Configuration.AddInMemoryCollection(initialData);
        builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", true, true);
        builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/hosting.json", true, true);

        var nacosOption = builder.Configuration.GetSection(NodeConsts.Nacos).Get<NacosConfig>();
        builder.Configuration.AddNacosConfiguration(nacosOption, true);


        var logOption = builder.Configuration.GetSection(NodeConsts.LogConfig).Get<LogConfig>();
        var logStashOption = builder.Configuration.GetSection(NodeConsts.LogStash).Get<LogStashConfig>();

        

        builder.UseDefaultSerilog(logOption,logStashOption);
        OnSettingConfigurationChanged(builder.Configuration);

 

        builder.Services.ReplaceConfiguration(builder.Configuration);
        builder.Services.AddSingleton(typeof(IServiceInfo), serviceInfo);
        builder.Services.AddService(serviceInfo);
        return builder;
    }

    /// <summary>
    /// Register Cofiguration ChangeCallback
    /// </summary>
    /// <param name="state"></param>
    private static IDisposable _callbackRegistration;
    private static void OnSettingConfigurationChanged(object state)
    {
        _callbackRegistration?.Dispose();
        var configuration = state as IConfiguration;
        var changedChildren = configuration.GetChildren();
        var reloadToken = configuration.GetReloadToken();

        ReplacePlaceholder(changedChildren);

        _callbackRegistration = reloadToken.RegisterChangeCallback(OnSettingConfigurationChanged, state);
    }

    /// <summary>
    /// replace placeholder
    /// </summary>
    /// <param name="sections"></param>
    private static void ReplacePlaceholder(IEnumerable<IConfigurationSection> sections)
    {
        var serviceInfo = ServiceInfo.GetInstance();
        foreach (var section in sections)
        {
            var childrenSections = section.GetChildren();
            if (childrenSections != null && childrenSections.Any())
                ReplacePlaceholder(childrenSections);

            if (string.IsNullOrWhiteSpace(section.Value))
                continue;

            var sectionValue = section.Value;
            if (sectionValue.Contains("$SERVICENAME"))
                section.Value = sectionValue.Replace("$SERVICENAME", serviceInfo.ServiceName);

            if (sectionValue.Contains("$SHORTNAME"))
                section.Value = sectionValue.Replace("$SHORTNAME", serviceInfo.ShortName);
        }
    }
}

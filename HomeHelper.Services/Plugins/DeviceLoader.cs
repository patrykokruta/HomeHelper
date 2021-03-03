using HomeHelper.SDK;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HomeHelper.Services.Plugins
{
    public static class DeviceLoader
    {
        public static List<IDevice> Load()
        {
            List<IDevice> deviceList = new List<IDevice>();

            var pluginDir = Path.Combine(AppContext.BaseDirectory, "plugins");

            if (!Directory.Exists(pluginDir))
            {
                return deviceList;
            }
            var assemblies = Directory.GetFiles(pluginDir, "*.dll");

            if (!assemblies.Any())
            {
                return deviceList;
            }
            foreach (var assembly in assemblies)
            {
                var loader = PluginLoader.CreateFromAssemblyFile(assembly, sharedTypes: new[]
                {
                    typeof(IApplicationBuilder),
                    typeof(IServiceCollection),
                    typeof(IDevice)
                });
                foreach (var type in loader.LoadDefaultAssembly().
                    GetTypes().Where(t => typeof(IDevice).IsAssignableFrom(t) && !t.IsInterface))
                {
                    var plugin = Activator.CreateInstance(type) as IDevice;
                    deviceList.Add(plugin);
                }
            }
            return deviceList;
        }
    }
}

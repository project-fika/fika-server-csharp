﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Utils;
using System.Text.Json;

namespace FikaServer.Services
{
    [Injectable(InjectionType.Singleton)]
    public class LocaleService(ISptLogger<LocaleService> logger, SPTarkov.Server.Core.Services.LocaleService localeService,
        JsonUtil jsonUtil, FileUtil fileUtil, ConfigService fikaConfig, DatabaseServer databaseServer)
    {
        private readonly string globalLocaleDir = Path.Join(fikaConfig.GetModPath(), "assets", "database", "locales", "global");
        //private readonly string serverLocaleDir = Path.Join(fikaConfig.GetModPath(), "assets", "database", "locales", "server");

        Dictionary<string, Dictionary<string, string>> _globalLocales = [];

        public async Task OnPostLoadAsync()
        {
            await LoadGlobalLocales();
            LoadServerLocales();
        }

        private async Task LoadGlobalLocales()
        {
            _globalLocales = await RecursiveLoadFiles(globalLocaleDir);

            foreach (var language in _globalLocales)
            {
                var languageLocales = language.Value;

                foreach (var locale in languageLocales)
                {
                    localeService.AddCustomClientLocale(language.Key, locale.Key, locale.Value);
                }
            }

            /* Todo
            foreach(var localeKvP in databaseServer.GetTables().Locales.Global)
            {
                localeKvP.Value.OnLazyLoad += (object? sender, SPTarkov.Server.Core.Utils.Json.OnLazyLoadEventArgs<Dictionary<string, string>> e) =>
                {
                    if(_globalLocales.ContainsKey(localeKvP.Key))
                    {
                        var fikaLocales = _globalLocales[localeKvP.Key];

                        foreach(var fikaLocale in fikaLocales)
                        {
                            if (!e.Value.ContainsKey(fikaLocale.Key))
                            {
                                e.Value.Add(fikaLocale.Key, fikaLocale.Value);
                            }
                        }
                    }
                };
            }
            */
        }

        private void LoadServerLocales()
        {
            // This is not necessary.. For now..
        }

        private async Task<Dictionary<string, Dictionary<string, string>>> RecursiveLoadFiles(string path)
        {
            List<string> files = fileUtil.GetFiles(path);

            Dictionary<string, Dictionary<string, string>> locales = [];

            foreach (string file in files)
            {
                await using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var localeFile = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);

                    locales.Add(Path.GetFileNameWithoutExtension(file), localeFile);
                }
            }

            return locales;
        }
    }
}

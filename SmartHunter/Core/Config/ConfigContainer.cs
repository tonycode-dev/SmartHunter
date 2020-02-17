using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SmartHunter.Game.Helpers;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace SmartHunter.Core.Config
{
    public class ConfigContainer<T> : FileContainer
    {
        public T Values = (T)Activator.CreateInstance(typeof(T), new object[] { });

        public event EventHandler Loaded;

        public ConfigContainer(string fileName) : base(fileName)
        {
            bool isDesignInstance = LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            if (!isDesignInstance)
            {
                Load();
            }
        }

        public void HandleDeserializationError(object sender, ErrorEventArgs args)
        {
            Log.WriteException(args.ErrorContext.Error);
            args.ErrorContext.Handled = true;
        }
        
        override protected void OnChanged()
        {
            Load();
        }

        void Load()
        {
            if (File.Exists(FullPathFileName))
            {
                try
                {
                    string contents = null;
                    using (var stream = File.Open(FullPathFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        contents = reader.ReadToEnd();
                    }

                    var fileContentsEqualsAutoGen = contents == GetAutoGenerateedJson();
                    if (!fileContentsEqualsAutoGen && FileName != "Config.json")
                    {
                        Log.WriteWarning($"Warning: {FileName} differs from autogenerated version.");
                    }

                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new ContractResolver(),
                        Error = HandleDeserializationError,
                    };

                    // Solves dictionary/lists being added to instead of overwritten but causes problems elsewhere
                    // https://stackoverflow.com/questions/29113063/json-net-why-does-it-add-to-list-instead-of-overwriting
                    // https://stackoverflow.com/questions/27848547/explanation-for-objectcreationhandling-using-newtonsoft-json
                    // This has been moved to ContractResolver to target Dictionaries specifically
                    // settings.ObjectCreationHandling = ObjectCreationHandling.Replace;
                    settings.Converters.Add(new StringEnumConverter());
                    settings.Converters.Add(new StringFloatConverter());

                    if (FileName.Equals("Config.json") || fileContentsEqualsAutoGen || ConfigHelper.Main.Values.UseCustomData)
                    {
                        JsonConvert.PopulateObject(contents, Values, settings);
                        Log.WriteLine($"{FileName} loaded");
                    }
                    else
                    {
                        Log.WriteLine($"{FileName} will be renamed to custom_{FileName} and recreated...");
                        Log.WriteLine("This can be disabled by setting [\"UseCustomData\": true,] in Config.json.");

                        File.Delete($"custom_{FileName}");
                        File.Move(FileName, $"custom_{FileName}");
                        Save();
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                }
            }
            else
            {
                Save();
            }

            Loaded?.Invoke(this, null);
        }

        public void Save(bool printToLog = true)
        {
            TryPauseWatching();

            try
            {
                File.WriteAllText(FullPathFileName, GetAutoGenerateedJson());
                if (printToLog)
                {
                    Log.WriteLine($"{FileName} saved");
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
            }

            TryUnpauseWatching();
        }

        private string GetAutoGenerateedJson()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new ContractResolver()
            };

            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new StringFloatConverter());

            return JsonConvert.SerializeObject(Values, settings);
        }
    }
}

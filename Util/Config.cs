using System.Collections;
using System.Reflection;
using DotNetEnv;
using Newtonsoft.Json;

namespace Util
{
    public static class Config
    {
        public struct Configuration
        {
            public string SymmetricKey { get; set; }
            public int TokenDurationMinutes { get; set; }
        }
        
        public static void LoadEnv(WebApplicationBuilder builder)
        {
            if(builder is null) throw new Exception("Builder not found");

            // Load ENV
            Env.Load("app.env");
            var envVars = Environment.GetEnvironmentVariables();
            var config = TransferToConfiguration(envVars);
            Console.WriteLine(config.SymmetricKey);
            //builder.Configuration.Bind(config);
        }

        private static Configuration TransferToConfiguration(IDictionary env)
        {
            ArgumentNullException.ThrowIfNull(env);

            Configuration config = new();

            foreach (var property in typeof(Configuration).GetProperties())
            {
                if (env.Contains(property.Name))
                {
                    var value = env[property.Name].ToString();
                    var convertedValue = ConvertToType(value, property.PropertyType);
                    Console.WriteLine(convertedValue);
                    property.SetValue(config, convertedValue);
                }
            }

            return config;
        }

        private static object ConvertToType(string value, Type targetType)
        {
            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (Exception ex) when (ex is FormatException || ex is InvalidCastException)
            {
                throw new Exception($"Failed to convert value '{value}' to type '{targetType}'.", ex);
            }
        }
    }
    
}
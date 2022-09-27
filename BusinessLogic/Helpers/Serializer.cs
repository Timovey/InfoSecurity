using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InfoSecurity1.BusinessLogic.Helpers
{
    //вспомогательный класс для безопасной сереализации в виде JSON
    public static class Serializer
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public static string SafeSerialize<T1>(this T1 obj, string defaultValue = null) where T1 : class
        {
            if (obj == null)
            {
                return defaultValue;
            }

            try
            {
                return JsonSerializer.Serialize(obj, _options);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T SafeDeserialize<T>(this string json, T defaultValue = null) where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                return defaultValue;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(json, _options);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}

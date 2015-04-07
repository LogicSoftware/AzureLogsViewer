using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace AzureLogsViewer.Web.Code.Json
{
    public static class JsonConversion
    {
        static JsonConversion()
        {
            Formatting = Formatting.None;
            Converters = new List<JsonConverter> { new StringEnumConverter() };
        }

        public static List<JsonConverter> Converters { get; set; }

        private static Formatting Formatting { get; set; }

        public static TValue Deserialize<TValue>(string json, params JsonConverter[] additionalConverters)
        {
            var serializer = GetSerializer(additionalConverters);
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return serializer.Deserialize<TValue>(jsonReader);
            }
        }

        public static object Deserialize(string json, Type objectType, params JsonConverter[] additionalConverters)
        {
            var serializer = GetSerializer(additionalConverters);
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                return serializer.Deserialize(jsonReader, objectType);
            }
        }

        public static TValue Deserialize<TValue>(JToken token, params JsonConverter[] additionalConverters)
        {
            var serializer = GetSerializer(additionalConverters);
            using (var jsonReader = token.CreateReader())
            {
                return serializer.Deserialize<TValue>(jsonReader);
            }
        }

        public static object Deserialize(JToken token, Type objectType, params JsonConverter[] additionalConverters)
        {
            var serializer = GetSerializer(additionalConverters);
            using (var jsonReader = token.CreateReader())
            {
                return serializer.Deserialize(jsonReader, objectType);
            }
        }

        private static JsonSerializer GetSerializer(params JsonConverter[] additionalConverters)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting,
                Converters = additionalConverters.Concat(Converters).ToList(),
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonSerializer.Create(settings);
        }

        public static string Serialize(object value, params JsonConverter[] additionalConverters)
        {
            var serializer = GetSerializer(additionalConverters);
            using (var stringWriter = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(stringWriter))
            {
                serializer.Serialize(jsonWriter, value);

                return stringWriter.ToString();
            }
        }
    }

}
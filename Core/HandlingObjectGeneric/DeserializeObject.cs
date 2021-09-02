using User.Backend.Api.Core.Enveloped;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace User.Backend.Api.Core.HandlingObjectGeneric
{
    public class DeserializeObject : IDeserializeObject
    {
        public T Deserialize<T>(EnvelopedObject.Enveloped ObjDeserialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();

            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(ObjDeserialize.body, settings));
        }

        public T DeserializeObjeto<T>(object ObjDeserialize)
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(ObjDeserialize, settings));
            }
            catch
            {
                return default(T);
            }
        }

        public T DeserializeJson<T>(string ObjDeserialize)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.DeserializeObject<T>(ObjDeserialize, settings);
        }
    }
}

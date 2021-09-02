using User.Backend.Api.Core.Enveloped;

namespace User.Backend.Api.Core.HandlingObjectGeneric
{
    public interface IDeserializeObject
    {
        T Deserialize<T>(EnvelopedObject.Enveloped ObjDeserialize);

        T DeserializeObjeto<T>(object ObjDeserialize);

        T DeserializeJson<T>(string ObjDeserialize);
    }
}

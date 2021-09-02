using User.Backend.Api.Core.Enveloped;

namespace User.Backend.Api.Core.HandlingObjectGeneric
{
    public interface IComposeObject
    {
        EnvelopedObject.Enveloped GetObject<T>(T Obj, string iniDate, string NameExpandoObject = null);
        EnvelopedObject.Enveloped GetObject_v2<T>(T Obj, string iniDate, string NameExpandoObject = null);
    }
}

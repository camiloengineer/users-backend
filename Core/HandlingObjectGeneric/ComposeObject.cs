
using User.Backend.Api.Core.Enveloped;
using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace User.Backend.Api.Core.HandlingObjectGeneric
{
    public class ComposeObject : IComposeObject
    {
        public EnvelopedObject.Enveloped GetObject<T>(T Obj, string iniDate, string NameExpandoObject = null)
        {
            string nameClass = ExtractClassName<T>(Obj, NameExpandoObject);

            dynamic DynamicObj = ConvertToDynamicObject<T>(nameClass, Obj);

            return new EnvelopedObject.Enveloped
            {
                Header = new EnvelopedObject.Header
                {
                    transactionData = new EnvelopedObject.Transactiondata
                    {
                        idTransaction = "WEB2019043000000100",
                        startDate = iniDate,
                        endDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                    }
                },
                body = DynamicObj
            };
        }

        public EnvelopedObject.Enveloped GetObject_v2<T>(T Obj, string iniDate, string NameExpandoObject = null)
        {
            dynamic DynamicObj = Obj;

            if (!string.IsNullOrWhiteSpace(NameExpandoObject))
            {
                string nameClass = ExtractClassName<T>(Obj, NameExpandoObject);

                DynamicObj = ConvertToDynamicObject<T>(nameClass, Obj);
            }

            return new EnvelopedObject.Enveloped
            {
                Header = new EnvelopedObject.Header
                {
                    transactionData = new EnvelopedObject.Transactiondata
                    {
                        startDate = iniDate,
                        endDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                    }
                },
                body = DynamicObj
            };
        }

        public string ExtractClassName<T>(T Obj, string NameExpandoObject = null)
        {
            string nameClass = string.Empty;

            try
            {
                nameClass = Obj.GetType().GetProperty("Item").PropertyType.Name;
            }
            catch
            {
                if (NameExpandoObject != null)
                    nameClass = NameExpandoObject;
                else
                {
                    if (Obj == null)
                        nameClass = NameExpandoObject;
                    else
                    {
                        nameClass = Obj.GetType().Name;

                        if (nameClass == "ExpandoObject")
                            nameClass = NameExpandoObject;
                    }
                }
            }
            return nameClass;
        }

        public dynamic ConvertToDynamicObject<T>(string nameClass, T Obj)
        {
            dynamic DynamicObj = new ExpandoObject();
            DynamicObj.Planillas = Obj;


            return DynamicObj;
        }
    }
}

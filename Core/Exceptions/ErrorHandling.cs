using User.Backend.Api.Core.Enveloped;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace User.Backend.Api.Core.Exceptions
{
    public class ErrorHandling
    {
        private readonly RequestDelegate next;
        private readonly JsonSerializerSettings _jsonSettings;
        private string DateInternalServerError;
        private readonly ILogger _log;

        public ErrorHandling(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            _log = loggerFactory.CreateLogger<ErrorHandling>();
            _jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task Invoke(HttpContext context)
        {
            string request = string.Empty;

            try
            {
                DateInternalServerError = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK");

                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _jsonSettings, request);

                if (ex is NotImplementedException) _log.LogError(0, ex, "" + ex.Message);
                else if (ex is UnauthorizedAccessException) _log.LogError(0, ex.Message);
                else if (ex is UserNotFoundException) _log.LogError(0, ex.Message);
                else if (ex is UserException) _log.LogError(0, ex, "" + ex.Message);
                else _log.LogError(0, ex, "Ocurrio una excepción no controlada:  " + ex.Message);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, JsonSerializerSettings Js, string request)
        {
            Stream originalBody = context.Response.Body;

            var bodyContent = string.Empty;
            context.Response.Body = originalBody;

            HttpStatusCode code = HttpStatusCode.InternalServerError;

            if (ex is NotImplementedException) code = HttpStatusCode.NotImplemented;
            else if (ex is UnauthorizedAccessException) code = HttpStatusCode.Unauthorized;
            else if (ex is UserNotFoundException) code = HttpStatusCode.NotFound;
            else if (ex is UserException) code = HttpStatusCode.BadRequest;

            using (var memStream = new MemoryStream())
            {
                context.Response.Body = memStream;
                memStream.Position = 0;

                var env = EnvelopedError(ex, context, code);

                bodyContent = JsonConvert.SerializeObject(env, Js);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;

                context.Response.WriteAsync(bodyContent, Encoding.UTF8);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                return memStream.CopyToAsync(originalBody);
            }
        }

        public EnvelopedObject.Enveloped EnvelopedError(Exception ex, HttpContext context, HttpStatusCode code)
        {
            List<string> levelErrorList = new List<string>() { "Technical", "Timeout ", "Functional" };
            string levelError = string.Empty;
            string typeError = string.Empty;
            string backendError = string.Empty;
            string codeError = string.Empty;
            string descriptionError = string.Empty;
            string inDate = string.Empty;

            if (ex.Message.IndexOf('$') > -1)
                inDate = ex.Message.Split('$')[1].Trim();

            if (ex.Message.IndexOf("ORA") > -1)
            {
                levelError = "Fatal";
                typeError = levelErrorList[2];
                backendError = ex.Message.Split(':')[1].Trim().Contains(" $ ") ? ex.Message.Split(':')[1].Trim().Split('$')[0].Trim() : ex.Message.Split(':')[1].Trim();
                codeError = ex.Message.Split(':')[0];
                descriptionError = ex.Message;
            }
            else if (ex.Message.IndexOf("TimeOut") > -1 || ex.Message.IndexOf("timed out") > -1)
            {
                levelError = "Fatal";
                typeError = levelErrorList[1];
                backendError = ex.Message;
                codeError = Convert.ToString((int)code + " - " + code.ToString());
                descriptionError = ex.Message;
                inDate = DateInternalServerError;
            }
            else
            {
                levelError = "Fatal";
                typeError = levelErrorList[0];
                backendError = ex.GetType().Name.Contains(" $ ") ? ex.GetType().Name.Split('$')[0] : ex.GetType().Name;
                codeError = Convert.ToString((int)code + " - " + code.ToString());
                descriptionError = ex.Message;
                inDate = DateInternalServerError;
            }

            var env = new EnvelopedObject.Enveloped
            {
                Header = new EnvelopedObject.Header
                {
                    transactionData = new EnvelopedObject.Transactiondata
                    {
                        idTransaction = "WEB2019043000000100",
                        startDate = inDate,
                        endDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")
                    }
                },
                body = new EnvelopedObject.Body
                {
                    error = new EnvelopedObject.Error
                    {
                        type = levelErrorList[2],
                        description = ex.Message.Split('$')[0].Trim(),
                        detail = new List<EnvelopedObject.Detail>
                        {
                            new EnvelopedObject.Detail
                            {
                                level = levelError,
                                type = typeError,
                                backend = backendError,
                                code = codeError,
                                description = descriptionError
                            }
                        }
                    }
                }
            };

            return env;
        }
    }
}

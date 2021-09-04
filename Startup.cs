using System;
using System.IO;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using User.Backend.Api.Clases.Mapping;
using User.Backend.Api.Core.Auth;
using User.Backend.Api.Core.Middleware;
using User.Backend.Api.Core.Services;

namespace User.Backend.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            EnvironmentVar conf = new EnvironmentVar();
            Configuration.Bind(conf);

            
            JObject secretsLocal = JObject.Parse("{}");

            if (File.Exists(@Configuration.GetValue<string>("SecretsLocal"))){
                secretsLocal  = JObject.Parse(File.ReadAllText(@Configuration.GetValue<string>("SecretsLocal")));
            }

            conf.UserAWS = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("UserAWS")) ? JObject.Parse(secretsLocal.ToString())["UserAWS"].ToString()  : Environment.GetEnvironmentVariable("UserAWS");
            conf.PasswordAWS = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PasswordAWS")) ? JObject.Parse(secretsLocal.ToString())["PasswordAWS"].ToString() : Environment.GetEnvironmentVariable("PasswordAWS");

            var credentials = new BasicAWSCredentials(conf.UserAWS, conf.PasswordAWS);
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = Amazon.RegionEndpoint.USEast1
            };

            var client = new AmazonDynamoDBClient(credentials, config);
            services.AddSingleton<IAmazonDynamoDB>(client);
            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddSingleton(Configuration);
            services.AddCors( o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            var configMapper = new AutoMapper.MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfileConfiguration()); });
            var mapper = configMapper.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Api User",
                    Version = "v1",
                    Description = "Servicio Backend de Usuarios.",
                    Contact = new OpenApiContact { Name = "Camilo González", Email = "cgonzalezm1234@gmail.com" }
                });
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddControllers();

            var key = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("EncryptionKey")) ? JObject.Parse(secretsLocal.ToString())["EncryptionKey"].ToString() : Environment.GetEnvironmentVariable("EncryptionKey");
            services.AddAuthentication(x =>
           {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer( x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(key));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("MyPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Net_Core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

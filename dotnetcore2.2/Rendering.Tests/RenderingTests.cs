using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Rendering.Models;
using Xunit;

namespace Rendering.Tests
{
    public class HostingOptions : IHostingOptions
    {
        public string ClientBasePath { get; set; } = "https://test.com";
        public string ServerBasePath { get; set; } = "https://test.com/backend";
    }

    public class RenderingTests
    {
        private readonly ComponentTestServerFixture _server;

        public RenderingTests()
        {
            _server = new ComponentTestServerFixture();
        }

        [Fact]
        public async Task Rendering_ConfirmEmail_FromModel_ExpectSuccess()
        {
            // Arrange
            var renderingApi = SetupRenderingApi();

            var searchResult = GenerateConfirmEmail();

            // Act
            var result = await renderingApi.RenderContent(searchResult);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("ConfirmEmail.json", typeof(ConfirmEmail), null)]
        public async Task Rendering_ConfirmEmail_FromJson_ExpectSuccess(string jsonFileName, Type jsonType, string name)
        {
            // Arrange
            var path = CalculatePath($@"Templates{Path.DirectorySeparatorChar}{jsonFileName}");

            var json = File.ReadAllText(path);
            var obj = JsonConvert.DeserializeObject(json, jsonType);
            var typedObj = Cast(jsonType, obj);

            var renderingApi = SetupRenderingApi();

            // Act
            //var result = renderingApi.RenderContent(typedObj, jsonType.Name); // Does not support dynamic created objects
            var renderingContentMethod = typeof(RenderingApi).GetMethod("RenderContent");
            var renderingContentOfJsonTypeMethod = renderingContentMethod.MakeGenericMethod(new[] { jsonType });
            var result = await (Task<string>)renderingContentOfJsonTypeMethod.Invoke(renderingApi, new object[] { typedObj, name });

            // Assert
            Assert.NotNull(result);
        }

        private static Func<object, object> MakeCastDelegate(Type from, Type to)
        {
            var p = Expression.Parameter(typeof(object)); //do not inline
            return Expression.Lambda<Func<object, object>>(
                Expression.Convert(Expression.ConvertChecked(Expression.Convert(p, from), to), typeof(object)),
                p).Compile();
        }

        private static readonly Dictionary<Tuple<Type, Type>, Func<object, object>> CastCache =
            new Dictionary<Tuple<Type, Type>, Func<object, object>>();

        public static Func<object, object> GetCastDelegate(Type from, Type to)
        {
            lock (CastCache)
            {
                var key = new Tuple<Type, Type>(from, to);
                Func<object, object> cast_delegate;
                if (!CastCache.TryGetValue(key, out cast_delegate))
                {
                    cast_delegate = MakeCastDelegate(from, to);
                    CastCache.Add(key, cast_delegate);
                }
                return cast_delegate;
            }
        }

        public static object Cast(Type t, object o)
        {
            return GetCastDelegate(o.GetType(), t).Invoke(o);
        }

        private static string CalculatePath(string relativePath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relativePath);
        }

        private RenderingApi SetupRenderingApi()
        {
            var serviceProvider = _server.GetRequiredService<IServiceProvider>();
            var viewEngine = _server.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = _server.GetRequiredService<ITempDataProvider>();

            var viewRenderer = new ViewRender(viewEngine, tempDataProvider, serviceProvider);
            var hostingOptions = new HostingOptions();
            var renderingApi = new RenderingApi(hostingOptions, viewRenderer);
            return renderingApi;
        }

        private static ConfirmEmail GenerateConfirmEmail()
        {
            return new ConfirmEmail
            {
                Email = "mail@test.com",
                Code = "code123",
                UserName = "user123"
            };
        }
    }
}

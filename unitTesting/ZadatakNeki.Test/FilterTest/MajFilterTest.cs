using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using ZadatakNeki.Filter;
using ZadatakNeki.Migrations;
using ZadatakNeki.Models;

namespace ZadatakNeki.Test.FilterTest
{
    public class MajFilterTest
    {
        public IActionResult ResultNovi()
        {
            return new OkObjectResult("Djesi");
        }
        
        [Fact]
        public void OnException_Test()
        {
            var _headers = new HeaderDictionary();

            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(mock => mock.Headers).Returns(_headers);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(mock => mock.Response).Returns(httpResponseMock.Object);
            var routeData = new Mock<RouteData>();
            var ad = new Mock<ActionDescriptor>();

            var actionContext = new ActionContext(httpContextMock.Object, routeData.Object, ad.Object);
            var lista = new Mock<IList<IFilterMetadata>>();

            var mockExContext = new Mock<ExceptionContext>(actionContext, lista.Object);
            mockExContext.Setup(e => e.Exception.Message).Returns("evo ti greska nek ima šuti");
            mockExContext.Setup(e => e.Exception.StackTrace).Returns("nista");
            mockExContext.SetupSet(e => e.ExceptionHandled);
            mockExContext.SetupSet(e => e.Result);

            var filter = new MajFilter();
            filter.OnException(mockExContext.Object);

            //Assert.IsType<VratiRezultat>();
        }

        [Fact]
        public void OnResultExecuting_Test()
        {
            var _headers = new HeaderDictionary();

            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(mock => mock.Headers).Returns(_headers);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(mock => mock.Response).Returns(httpResponseMock.Object);

            var routeData = new Mock<RouteData>();
            var ad = new Mock<ActionDescriptor>();

            var actionContext = new ActionContext(httpContextMock.Object, routeData.Object, ad.Object);
            var mt = new Mock<IList<IFilterMetadata>>();
            var ar = new Mock<ActionResult>();
            object c = null;

            var context = new Mock<ResultExecutingContext>(actionContext, mt.Object, ar.Object, c);
            context.Setup(e => e.Result).Returns(ResultNovi);
            context.SetupSet(e => e.Result);

            var filter = new MajFilter();
            filter.OnResultExecuting(context.Object);

            var result = context.Object.Result as ObjectResult;

            //Assert.IsType<VratiRezultat>(result.ContentTypes.GetType());
        }
    }
}

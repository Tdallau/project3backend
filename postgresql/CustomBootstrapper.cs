using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace postgresql
{
    public class CustomBootstrapper : AutofacNancyBootstrapper {
		protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
        pipelines.AfterRequest += ctx =>
        {
            ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            ctx.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, Authorization");
            ctx.Response.Headers.Add("Access-Control-Allow-Methods", "DELETE");
        };
        }
    }
}
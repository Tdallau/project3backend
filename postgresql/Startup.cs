using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;
using postgresql.modules;
using System.Configuration;

namespace postgresql
{
  public class Startup
  {
    private IConfiguration Configuration { get; }
    public static string ConnectionString { get; private set; }
    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //var test = new Backend(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      ConnectionString = this.Configuration.GetConnectionString("testDatabase");
      
      app.UseOwin(x => x.UseNancy());


    }
  }
}

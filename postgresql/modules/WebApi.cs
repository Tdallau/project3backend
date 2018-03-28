using Microsoft.Extensions.Configuration;
using Nancy;
using Nancy.ModelBinding;
using Npgsql;
using postgresql.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace postgresql.modules
{
  public class WebApi : NancyModule
  {
        private NpgsqlConnection conn = new NpgsqlConnection(Startup.ConnectionString);

        private Func<string,string,string> getWithDefault = (post,defaultVal) => {
            return post != null ? post : defaultVal;
        };

        private Func<int,int,int> getWithDefaultInt = (post,defaultVal) => {
            return ((post == 1) || (post == 0)) ? post : defaultVal;
        };

        public WebApi() {
            Post("api/crime", parameters =>
            {
                var post = this.Bind<Crime>();
                var list = Crime.getCrime(
                    this.conn,
                    this.getWithDefault(post.Crime_end_type,"Geregistreerde misdrijven"),
                    this.getWithDefault(post.Crime_type,"totaal")
                );
                return Response.AsJson(list);
            });

            Post("api/work", parameters =>
            {
                var post = this.Bind<Work>();
                var list = Work.getWork(
                    this.conn,
                    this.getWithDefault(post.worker_type_name,"Banen"),
                    this.getWithDefaultInt(post.isseasoncorrected,0),
                    this.getWithDefault(post.value_type_name,"Totaal"),
                    this.getWithDefault(post.branch_name,"A-U Alle economische activiteiten")
                );
                return Response.AsJson(list);
            });

            Post("api/education", parameters =>
            {
                var post = this.Bind<Education>();
                var list = Education.getEducation(
                    this.conn,
                    this.getWithDefault(post.gender,"Totaal mannen en vrouwen"),
                    this.getWithDefault(post.educationType,"Totaal")
                );
                return Response.AsJson(list);
            });

            Get("api/education-types", parmeters => {
                var list = Types.getTypes(conn,"SELECT id, name FROM education_type");
                return Response.AsJson(list);
            });
            Get("api/crime-types", parmeters => {
                var list = Types.getTypes(conn,"SELECT id, name FROM crime_type");
                return Response.AsJson(list);
            });

        }

  }
}
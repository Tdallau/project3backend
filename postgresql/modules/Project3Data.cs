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
  public class Project3Data : NancyModule
  {
    private NpgsqlConnection conn = new NpgsqlConnection(Startup.ConnectionString);
    public Project3Data()
    {
      Post("api/crime", parameters =>
      {
        var postData = this.Bind<CrimePost>();
        var crime_end_type = "Geregistreerde misdrijven";
        
        if(postData.crime_end_type != null) {
          crime_end_type = postData.crime_end_type;
        } 
        var list = getCrime(crime_end_type);
        return Response.AsJson(list);
      });

      Post("api/work", parameters =>
      {
        var postData = this.Bind<WorkPost>();
        var wvtName = "Banen";

        if(postData.wvtName != null) {
          wvtName = postData.wvtName;
        }

        var list = getWork(wvtName);
        return Response.AsJson(list);
      });

      Post("api/education", parameters =>
      {
        var postData = this.Bind<EducationPost>();
        var gender = "Totaal mannen en vrouwen";

        if(postData.gender != null) {
          gender = postData.gender;
        }

        var list = getEducation(gender);
        return Response.AsJson(list);
      });

    }

    private List<Crime> getCrime(string crime_end_type)
    {
      conn.Open();

      string sql = "SELECT crime_value.id as Id, amount, year, ct.name as crime_type, ct2.name as parent_crime, cet.name as crime_end_type "+
      "FROM crime_value, crime_end_type as cet, crime_type as ct "+
      "FULL JOIN crime_type as ct2 ON ct2.id = ct.parent_id "+
      "WHERE cet.id = crime_value.crime_end_type "+
      "AND ct.id = crime_value.crime_type "+
      "AND ct2.name IS null AND cet.name = :cetName AND ct.name = 'totaal'"+
      "ORDER BY year";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
      command.Parameters.Add(new  NpgsqlParameter("cetName", crime_end_type));
      NpgsqlDataReader dr = command.ExecuteReader();

      var result = new List<Crime>();
      // Output rows
      while (dr.Read())
      {
        result.Add(Crime.FromDataReader(dr));
      }
      conn.Close();
       
      return result;
    }
    private List<Work> getWork(string wvtName)
    {
      conn.Open();

      string sql = "SELECT work_value.id as id, work_value.isseasoncorrected as isseasoncorrected, work_value.amount as amount, work_date.year as year, work_date.period as period, work_branch.name as branch_name, work_worker_type.name as worker_type_name, work_value_type.name as value_type_name "+
      "FROM work_value "+
      "JOIN work_date ON work_value.workdateid = work_date.id "+
      "JOIN work_branch ON work_date.branchid = work_branch.id "+
      "JOIN work_worker_type ON work_date.workertypeid = work_worker_type.id "+
      "JOIN work_value_type ON work_value.amounttype = work_value_type.id "+
      "WHERE period IS null AND work_value.isseasoncorrected != 1 and work_value_type.name = :wvtName AND work_worker_type.name = 'Totaal' AND work_branch.name = 'A-U Alle economische activiteiten' "+
      "ORDER BY year";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
      command.Parameters.Add(new NpgsqlParameter("wvtName", wvtName));
      NpgsqlDataReader dr = command.ExecuteReader();

      var result = new List<Work>();
      // Output rows
      while (dr.Read())
      {
        result.Add(Work.FromDataReader(dr));
      }
      conn.Close();
      return result;
    }
    private List<Education> getEducation(string gender)
    {
      conn.Open();

      string sql = "SELECT education_value.id as id, year, amount, education_gender.name as gender, education_type.name as educationType "+
      "FROM education_value, education_gender, education_type "+
      "WHERE education_value.genderid = education_gender.id AND "+
      "education_value.educationtype = education_type.id "+
      "AND education_gender.name = :gender AND education_type.name = 'Totaal' "+
      "ORDER BY year";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
      command.Parameters.Add(new NpgsqlParameter("gender", gender));
      NpgsqlDataReader dr = command.ExecuteReader();

      var result = new List<Education>();
      // Output rows
      while (dr.Read())
      {
        result.Add(Education.FromDataReader(dr));
      }
      conn.Close();
       
      return result;
    }
}
}

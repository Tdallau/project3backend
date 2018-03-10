using Microsoft.Extensions.Configuration;
using Nancy;
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
      Get("api/crime", parameters =>
      {
        var list = getCrime();
        return Response.AsJson(list);
      });

      Get("api/work", parameters =>
      {
        var list = getWork();
        return Response.AsJson(list);
      });

      Get("api/education", parameters =>
      {
        var list = getEducation();
        return Response.AsJson(list);
      });

    }

    private List<Crime> getCrime()
    {
      conn.Open();

      string sql = "SELECT crime_value.id as id, amount, year, ct.name as crime_type, ct2.name as parent_crime, crime_end_type.name as crime_end_type " +
                   "FROM crime_value, crime_end_type, crime_type as ct " +
                   "FULL JOIN crime_type as ct2 ON ct.id = ct2.parent_id " +
                   "WHERE ct.id = crime_value.crime_type AND " +
                   "crime_value.crime_end_type = crime_end_type.id";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
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
    private List<Work> getWork()
    {
      conn.Open();

      string sql = "SELECT work_value.id as id, work_value.isseasoncorrected as isseasoncorrected, work_value.amount as amount, work_date.year as year, work_date.period as period, work_branch.name as branch_name, work_worker_type.name as worker_type_name, work_value_type.name as value_type_name "+
      "FROM work_value "+
      "JOIN work_date ON work_value.workdateid = work_date.id "+
      "JOIN work_branch ON work_date.branchid = work_branch.id "+
      "JOIN work_worker_type ON work_date.workertypeid = work_worker_type.id "+
      "JOIN work_value_type ON work_value.amounttype = work_value_type.id";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
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
    private List<Education> getEducation()
    {
      conn.Open();

      string sql = "SELECT education_value.id as id, year, amount, education_gender.name as gender, education_type.name as educationType "+
      "FROM education_value, education_gender, education_type "+
      "WHERE education_value.genderid = education_gender.id AND "+
      "education_value.educationtype = education_type.id";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
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

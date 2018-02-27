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
       
      return result;
    }
}
}

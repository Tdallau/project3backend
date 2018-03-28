using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace postgresql.model
{
  public class Crime
  {
    public int Id { get; set; }
    public int Amount { get; set; }
    public int Year { get; set; }
    public string Crime_type { get; set; }
    public string parent_crime { get; set; }
    public string Crime_end_type { get; set;  }
    public string label {get; set; }

    public string color {get; set;}

    public static Crime FromDataReader(IDataReader dr)
    {
      return new Crime()
      {
        Id = Convert.ToInt32(dr["Id"]),
        Amount = Convert.ToInt32(dr["amount"]),
        Year = Convert.ToInt32(dr["year"]),
        Crime_type = dr["crime_type"].ToString(),
        parent_crime = dr["parent_crime"].ToString(),
        Crime_end_type = dr["crime_end_type"].ToString(),
        label = "Crime",
        color = "#3cba9f"
      };
    }

    public static List<Crime> getCrime(NpgsqlConnection conn, string crime_end_type, string crime_type_name) {
      conn.Open();

      string sql = @"SELECT 
        crime_value.id as Id,
        amount,
        year,
        ct.name as crime_type,
        ct2.name as parent_crime,
        cet.name as crime_end_type
      FROM
        crime_value,
        crime_end_type as cet,
        crime_type as ct
      FULL JOIN 
        crime_type as ct2
        ON ct2.id = ct.parent_id
      WHERE cet.id = crime_value.crime_end_type
      AND ct.id = crime_value.crime_type
      AND cet.name = :cetName 
      AND ct.name = :crimeTypeName
      
      ORDER BY year";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
      command.Parameters.Add(new  NpgsqlParameter("cetName", crime_end_type));
      command.Parameters.Add(new  NpgsqlParameter("crimeTypeName", crime_type_name));
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

  }

}

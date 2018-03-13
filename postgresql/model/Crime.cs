using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

  }

}

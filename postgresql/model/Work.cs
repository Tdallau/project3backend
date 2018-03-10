using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace postgresql.model
{
  public class Work
  {
    public int id { get; set; }
    public int isseasoncorrected { get; set; }
    public string amount { get; set; }
    public int year { get; set; }
    public string period { get; set; }
    public string branch_name { get; set;  }
    public string worker_type_name { get; set;  }
    public string value_type_name { get; set;  }

    public static Work FromDataReader(IDataReader dr)
    {
      return new Work()
      {
        id = Convert.ToInt32(dr["id"]),
        isseasoncorrected = Convert.ToInt32(dr["isseasoncorrected"]),
        amount = dr["amount"].ToString(),
        year = Convert.ToInt32(dr["year"]),
        period = dr["period"].ToString(),
        branch_name = dr["branch_name"].ToString(),
        worker_type_name  = dr["worker_type_name"].ToString(),
        value_type_name = dr["value_type_name"].ToString()
      };
    }

  }

}

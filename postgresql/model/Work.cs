using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

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
    public string label {get; set; }
    public string color {get; set;}

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
        value_type_name = dr["value_type_name"].ToString(),
        label = "Work",
        color = "#3fda9f"
      };
    }

    public static List<Work> getWork(NpgsqlConnection conn,string wvtName,int isSeasonCorrected,string typeName,string branch) {
      conn.Open();

      string sql = @"SELECT 
        work_value.id as id, 
        work_value.isseasoncorrected as isseasoncorrected, 
        work_value.amount as amount, 
        work_date.year as year, 
        work_date.period as period, 
        work_branch.name as branch_name, 
        work_worker_type.name as worker_type_name, 
        work_value_type.name as value_type_name
      FROM work_value
      JOIN work_date
      ON work_value.workdateid = work_date.id
      JOIN work_branch
      ON work_date.branchid = work_branch.id
      JOIN work_worker_type
      ON work_date.workertypeid = work_worker_type.id
      JOIN work_value_type
      ON work_value.amounttype = work_value_type.id
      
      WHERE work_value.isseasoncorrected = :isSeasonCorrected
      AND work_date.period IS NULL 
      AND work_value_type.name = :wvtName
      AND work_worker_type.name = :typeName
      AND work_branch.name = :branch
      ORDER BY year";

      NpgsqlCommand command = new NpgsqlCommand(sql, conn);
      command.Parameters.Add(new NpgsqlParameter("wvtName", wvtName));
      command.Parameters.Add(new NpgsqlParameter("isSeasonCorrected", isSeasonCorrected));
      command.Parameters.Add(new NpgsqlParameter("typeName", typeName));
      command.Parameters.Add(new NpgsqlParameter("branch", branch));
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

  }

}

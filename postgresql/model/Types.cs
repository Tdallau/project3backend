using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace postgresql.model
{
    public class Types {

        public int id {get; set;}
        public string name {get; set;}

        public static Types FromDataReader(IDataReader dr)
        {
            return new Types()
            {
                id = Convert.ToInt32(dr["id"]),
                name = dr["name"].ToString()
            };
        }

        public static List<Types> getTypes(NpgsqlConnection conn, string sql) {
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader dr = command.ExecuteReader();

            var result = new List<Types>();
            // Output rows
            while (dr.Read())
            {
                result.Add(Types.FromDataReader(dr));
            }
            conn.Close();
            
            return result;
        }
    }
}
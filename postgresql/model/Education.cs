using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace postgresql.model
{
    public class Education
    {
        public int id { get; set; }
        public int year { get; set; }
        public int amount { get; set; }
        public string gender { get; set; }
        public string educationType { get; set; }
        public string label { get; set; }
        public string color { get; set; }
        public static Education FromDataReader(IDataReader dr)
        {
            return new Education()
            {
                id = Convert.ToInt32(dr["id"]),
                year = Convert.ToInt32(dr["year"]),
                amount = Convert.ToInt32(dr["amount"]),
                gender = dr["gender"].ToString(),
                educationType = dr["educationType"].ToString(),
                label = "Education",
                color = "#3fda0a"
            };
        }

        public static List<Education> getEducation(NpgsqlConnection conn, string gender, string eduTypeName)
        {
            conn.Open();

            string sql = @"SELECT 
        education_value.id as id, 
        year,
        amount,
        education_gender.name as gender,
        education_type.name as educationType
      FROM education_value, education_gender, education_type
      WHERE education_value.genderid = education_gender.id
      AND education_value.educationtype = education_type.id
      AND education_gender.name = :gender 
      AND education_type.name = :eduTypeName
      ORDER BY year";

            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            command.Parameters.Add(new NpgsqlParameter("gender", gender));

            command.Parameters.Add(new NpgsqlParameter("eduTypeName", eduTypeName));
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
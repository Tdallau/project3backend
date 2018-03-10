using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace postgresql.model
{
    public class Education {
        public int id {get; set;}
        public int year {get; set;}
        public int amount {get; set;}
        public string gender {get; set;}
        public string educationType { get; set;}
        public static Education FromDataReader(IDataReader dr)
        {
            return new Education()
            {
                id = Convert.ToInt32(dr["id"]),
                year = Convert.ToInt32(dr["year"]),
                amount = Convert.ToInt32(dr["amount"]),
                gender = dr["gender"].ToString(),
                educationType = dr["educationType"].ToString(),
            };
        }
    }
}
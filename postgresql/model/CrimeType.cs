using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace postgresql.model
{
    public class CrimeType {

        public int id {get; set;}
        public string name {get; set;}

        public static CrimeType FromDataReader(IDataReader dr)
        {
            return new CrimeType()
            {
                id = Convert.ToInt32(dr["id"]),
                name = dr["name"].ToString()
            };
        }
    }
}
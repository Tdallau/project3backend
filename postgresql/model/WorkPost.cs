using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace postgresql.model
{
  public class WorkPost
  {
      public string wvtName {get; set;}
      public int isSeasonCorrected {get; set;}
      public string branch {get; set;}
      public string typeName{get; set;}
  }
}

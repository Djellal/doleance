using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doleance.Models.AppDb
{
  [Table("Qualite")]
  public partial class Qualite
  {
    public string Designation
    {
      get;
      set;
    }
  }
}

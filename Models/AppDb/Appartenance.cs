using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doleance.Models.AppDb
{
  [Table("Appartenance")]
  public partial class Appartenance
  {
    [Key]
    public int? Id
    {
      get;
      set;
    }
    public string Designation
    {
      get;
      set;
    }
  }
}

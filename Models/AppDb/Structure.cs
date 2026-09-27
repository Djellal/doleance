using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doleance.Models.AppDb
{
  [Table("Structure")]
  public partial class Structure
  {
    [Key]
    public int? Id
    {
      get;
      set;
    }

    public ICollection<Rendezvou> Rendezvous { get; set; }
    public string Designation
    {
      get;
      set;
    }
  }
}

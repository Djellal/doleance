using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doleance.Models.AppDb
{
  [Table("doleanctab")]
  public partial class Doleanctab
  {
    [Key]
    public int? Id
    {
      get;
      set;
    }
    public string objet
    {
      get;
      set;
    }
    public string nomPrenom
    {
      get;
      set;
    }
    public string paragraph
    {
      get;
      set;
    }
    public DateTime? date
    {
      get;
      set;
    }
    public string email
    {
      get;
      set;
    }
    public string userid
    {
      get;
      set;
    }
    public string reponse
    {
      get;
      set;
    }
    public bool repondu
    {
      get;
      set;
    }
    public string num
    {
      get;
      set;
    }
    public int? structId
    {
      get;
      set;
    }
  }
}

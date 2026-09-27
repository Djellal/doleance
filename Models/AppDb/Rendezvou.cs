using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doleance.Models.AppDb
{
  [Table("rendezvous")]
  public partial class Rendezvou
  {
    [Key]
    public int? Id
    {
      get;
      set;
    }
    public string num
    {
      get;
      set;
    }
    public DateTime? date
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
    public DateTime? dateDemandee
    {
      get;
      set;
    }
    public bool acceptee
    {
      get;
      set;
    }
    public DateTime? dateRdv
    {
      get;
      set;
    }
    public string textReponse
    {
      get;
      set;
    }
    public int? structId
    {
      get;
      set;
    }
    public Structure Structure { get; set; }
    public string userid
    {
      get;
      set;
    }
    public string email
    {
      get;
      set;
    }
  }
}

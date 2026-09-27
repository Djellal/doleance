using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Doleance.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Qualite { get; set; }
        public bool Memberofufas { get; set; } = false;
        public string Affiliation { get; set; }
        public int? StructId { get; set; }

    }
}
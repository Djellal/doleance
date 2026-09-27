using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using Doleance.Data;

namespace Doleance
{
    public partial class AppDbService
        {
        public async Task<string> GetNewNumInscription()
        {
            
            return "D-"+(context.Doleanctabs.Count() + 1).ToString("D4");
             
        }

        public async Task<string> GetNewNumRdv()
        {

            return "RDV-" + (context.Rendezvous.Count() + 1).ToString("D4");

        }
    }
}

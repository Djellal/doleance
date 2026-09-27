using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;

namespace Doleance.Pages
{
    public partial class DoleanctabComponent
    {
        protected async System.Threading.Tasks.Task FilterDoleance()
        {
            
            
            switch (filter)
            {
                case 0:
                    if (Security.IsInRole(Constants.admin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d=>!d.repondu).OrderByDescending(d => d.date); 

                    }
                    else if (Security.IsInRole(Constants.structadmin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.structId == CurrentUser.StructId && !d.repondu).OrderByDescending(d => d.date);

                    }
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.userid == CurrentUser.Id && !d.repondu).OrderByDescending(d => d.date); ;

                    };
                    break;
                case 1:
                    if (Security.IsInRole(Constants.admin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.repondu).OrderByDescending(d => d.date); 

                    }
                    else if (Security.IsInRole(Constants.structadmin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.structId == CurrentUser.StructId && d.repondu).OrderByDescending(d => d.date);

                    }
                    else
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.userid == CurrentUser.Id && d.repondu).OrderByDescending(d => d.date);

                    };
                    break;
                case 2:
                    if (Security.IsInRole(Constants.admin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).OrderByDescending(d => d.date);

                    }
                    else if (Security.IsInRole(Constants.structadmin))
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.structId == CurrentUser.StructId).OrderByDescending(d => d.date);

                    }
                    else
                    {
                        getDoleanctabsResult = (await AppDb.GetDoleanctabs()).Where(d => d.userid == CurrentUser.Id).OrderByDescending(d => d.date);

                    };
                    break;
               
            }

            if(SelectedStucture != null)
            {
                getDoleanctabsResult = getDoleanctabsResult.Where(d => d.structId == SelectedStucture);
            }

            await grid0.Reload();
        }

    }
}

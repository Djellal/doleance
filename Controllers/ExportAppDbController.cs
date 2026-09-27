using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doleance.Data;

namespace Doleance
{
    public partial class ExportAppDbController : ExportController
    {
        private readonly AppDbContext context;
        private readonly AppDbService service;
        public ExportAppDbController(AppDbContext context, AppDbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/AppDb/appartenances/csv")]
        [HttpGet("/export/AppDb/appartenances/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAppartenancesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAppartenances(), Request.Query, false), fileName);
        }

        [HttpGet("/export/AppDb/appartenances/excel")]
        [HttpGet("/export/AppDb/appartenances/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportAppartenancesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAppartenances(), Request.Query, false), fileName);
        }
        [HttpGet("/export/AppDb/doleanctabs/csv")]
        [HttpGet("/export/AppDb/doleanctabs/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportDoleanctabsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDoleanctabs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/AppDb/doleanctabs/excel")]
        [HttpGet("/export/AppDb/doleanctabs/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportDoleanctabsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDoleanctabs(), Request.Query, false), fileName);
        }
        [HttpGet("/export/AppDb/qualites/csv")]
        [HttpGet("/export/AppDb/qualites/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportQualitesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetQualites(), Request.Query, true), fileName);
        }

        [HttpGet("/export/AppDb/qualites/excel")]
        [HttpGet("/export/AppDb/qualites/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportQualitesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetQualites(), Request.Query, true), fileName);
        }
        [HttpGet("/export/AppDb/rendezvous/csv")]
        [HttpGet("/export/AppDb/rendezvous/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportRendezvousToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetRendezvous(), Request.Query, false), fileName);
        }

        [HttpGet("/export/AppDb/rendezvous/excel")]
        [HttpGet("/export/AppDb/rendezvous/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportRendezvousToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetRendezvous(), Request.Query, false), fileName);
        }
        [HttpGet("/export/AppDb/structures/csv")]
        [HttpGet("/export/AppDb/structures/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportStructuresToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStructures(), Request.Query, false), fileName);
        }

        [HttpGet("/export/AppDb/structures/excel")]
        [HttpGet("/export/AppDb/structures/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportStructuresToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStructures(), Request.Query, false), fileName);
        }
    }
}

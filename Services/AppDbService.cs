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
        AppDbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly AppDbContext context;
        private readonly NavigationManager navigationManager;

        public AppDbService(AppDbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportAppartenancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/appartenances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/appartenances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAppartenancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/appartenances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/appartenances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAppartenancesRead(ref IQueryable<Models.AppDb.Appartenance> items);

        public async Task<IQueryable<Models.AppDb.Appartenance>> GetAppartenances(Query query = null)
        {
            var items = Context.Appartenances.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAppartenancesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAppartenanceCreated(Models.AppDb.Appartenance item);
        partial void OnAfterAppartenanceCreated(Models.AppDb.Appartenance item);

        public async Task<Models.AppDb.Appartenance> CreateAppartenance(Models.AppDb.Appartenance appartenance)
        {
            OnAppartenanceCreated(appartenance);

            var existingItem = Context.Appartenances
                              .Where(i => i.Id == appartenance.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Appartenances.Add(appartenance);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(appartenance).State = EntityState.Detached;
                throw;
            }

            OnAfterAppartenanceCreated(appartenance);

            return appartenance;
        }
        public async Task ExportDoleanctabsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/doleanctabs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/doleanctabs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDoleanctabsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/doleanctabs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/doleanctabs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDoleanctabsRead(ref IQueryable<Models.AppDb.Doleanctab> items);

        public async Task<IQueryable<Models.AppDb.Doleanctab>> GetDoleanctabs(Query query = null)
        {
            var items = Context.Doleanctabs.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnDoleanctabsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDoleanctabCreated(Models.AppDb.Doleanctab item);
        partial void OnAfterDoleanctabCreated(Models.AppDb.Doleanctab item);

        public async Task<Models.AppDb.Doleanctab> CreateDoleanctab(Models.AppDb.Doleanctab doleanctab)
        {
            OnDoleanctabCreated(doleanctab);

            var existingItem = Context.Doleanctabs
                              .Where(i => i.Id == doleanctab.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Doleanctabs.Add(doleanctab);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(doleanctab).State = EntityState.Detached;
                throw;
            }

            OnAfterDoleanctabCreated(doleanctab);

            return doleanctab;
        }
        public async Task ExportQualitesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/qualites/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/qualites/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportQualitesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/qualites/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/qualites/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnQualitesRead(ref IQueryable<Models.AppDb.Qualite> items);

        public async Task<IQueryable<Models.AppDb.Qualite>> GetQualites(Query query = null)
        {
            var items = Context.Qualites.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnQualitesRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task ExportRendezvousToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/rendezvous/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/rendezvous/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRendezvousToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/rendezvous/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/rendezvous/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRendezvousRead(ref IQueryable<Models.AppDb.Rendezvou> items);

        public async Task<IQueryable<Models.AppDb.Rendezvou>> GetRendezvous(Query query = null)
        {
            var items = Context.Rendezvous.AsQueryable();

            items = items.Include(i => i.Structure);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnRendezvousRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRendezvouCreated(Models.AppDb.Rendezvou item);
        partial void OnAfterRendezvouCreated(Models.AppDb.Rendezvou item);

        public async Task<Models.AppDb.Rendezvou> CreateRendezvou(Models.AppDb.Rendezvou rendezvou)
        {
            OnRendezvouCreated(rendezvou);

            var existingItem = Context.Rendezvous
                              .Where(i => i.Id == rendezvou.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Rendezvous.Add(rendezvou);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(rendezvou).State = EntityState.Detached;
                rendezvou.Structure = null;
                throw;
            }

            OnAfterRendezvouCreated(rendezvou);

            return rendezvou;
        }
        public async Task ExportStructuresToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/structures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/structures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStructuresToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/appdb/structures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/appdb/structures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStructuresRead(ref IQueryable<Models.AppDb.Structure> items);

        public async Task<IQueryable<Models.AppDb.Structure>> GetStructures(Query query = null)
        {
            var items = Context.Structures.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStructuresRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStructureCreated(Models.AppDb.Structure item);
        partial void OnAfterStructureCreated(Models.AppDb.Structure item);

        public async Task<Models.AppDb.Structure> CreateStructure(Models.AppDb.Structure structure)
        {
            OnStructureCreated(structure);

            var existingItem = Context.Structures
                              .Where(i => i.Id == structure.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Structures.Add(structure);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(structure).State = EntityState.Detached;
                throw;
            }

            OnAfterStructureCreated(structure);

            return structure;
        }

        partial void OnAppartenanceDeleted(Models.AppDb.Appartenance item);
        partial void OnAfterAppartenanceDeleted(Models.AppDb.Appartenance item);

        public async Task<Models.AppDb.Appartenance> DeleteAppartenance(int? id)
        {
            var itemToDelete = Context.Appartenances
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAppartenanceDeleted(itemToDelete);

            Context.Appartenances.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAppartenanceDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnAppartenanceGet(Models.AppDb.Appartenance item);

        public async Task<Models.AppDb.Appartenance> GetAppartenanceById(int? id)
        {
            var items = Context.Appartenances
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnAppartenanceGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AppDb.Appartenance> CancelAppartenanceChanges(Models.AppDb.Appartenance item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAppartenanceUpdated(Models.AppDb.Appartenance item);
        partial void OnAfterAppartenanceUpdated(Models.AppDb.Appartenance item);

        public async Task<Models.AppDb.Appartenance> UpdateAppartenance(int? id, Models.AppDb.Appartenance appartenance)
        {
            OnAppartenanceUpdated(appartenance);

            var itemToUpdate = Context.Appartenances
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(appartenance);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterAppartenanceUpdated(appartenance);

            return appartenance;
        }

        partial void OnDoleanctabDeleted(Models.AppDb.Doleanctab item);
        partial void OnAfterDoleanctabDeleted(Models.AppDb.Doleanctab item);

        public async Task<Models.AppDb.Doleanctab> DeleteDoleanctab(int? id)
        {
            var itemToDelete = Context.Doleanctabs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDoleanctabDeleted(itemToDelete);

            Context.Doleanctabs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDoleanctabDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnDoleanctabGet(Models.AppDb.Doleanctab item);

        public async Task<Models.AppDb.Doleanctab> GetDoleanctabById(int? id)
        {
            var items = Context.Doleanctabs
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnDoleanctabGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AppDb.Doleanctab> CancelDoleanctabChanges(Models.AppDb.Doleanctab item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDoleanctabUpdated(Models.AppDb.Doleanctab item);
        partial void OnAfterDoleanctabUpdated(Models.AppDb.Doleanctab item);

        public async Task<Models.AppDb.Doleanctab> UpdateDoleanctab(int? id, Models.AppDb.Doleanctab doleanctab)
        {
            OnDoleanctabUpdated(doleanctab);

            var itemToUpdate = Context.Doleanctabs
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(doleanctab);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterDoleanctabUpdated(doleanctab);

            return doleanctab;
        }

        partial void OnRendezvouDeleted(Models.AppDb.Rendezvou item);
        partial void OnAfterRendezvouDeleted(Models.AppDb.Rendezvou item);

        public async Task<Models.AppDb.Rendezvou> DeleteRendezvou(int? id)
        {
            var itemToDelete = Context.Rendezvous
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRendezvouDeleted(itemToDelete);

            Context.Rendezvous.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRendezvouDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnRendezvouGet(Models.AppDb.Rendezvou item);

        public async Task<Models.AppDb.Rendezvou> GetRendezvouById(int? id)
        {
            var items = Context.Rendezvous
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Structure);

            var itemToReturn = items.FirstOrDefault();

            OnRendezvouGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AppDb.Rendezvou> CancelRendezvouChanges(Models.AppDb.Rendezvou item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnRendezvouUpdated(Models.AppDb.Rendezvou item);
        partial void OnAfterRendezvouUpdated(Models.AppDb.Rendezvou item);

        public async Task<Models.AppDb.Rendezvou> UpdateRendezvou(int? id, Models.AppDb.Rendezvou rendezvou)
        {
            OnRendezvouUpdated(rendezvou);

            var itemToUpdate = Context.Rendezvous
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(rendezvou);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterRendezvouUpdated(rendezvou);

            return rendezvou;
        }

        partial void OnStructureDeleted(Models.AppDb.Structure item);
        partial void OnAfterStructureDeleted(Models.AppDb.Structure item);

        public async Task<Models.AppDb.Structure> DeleteStructure(int? id)
        {
            var itemToDelete = Context.Structures
                              .Where(i => i.Id == id)
                              .Include(i => i.Rendezvous)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStructureDeleted(itemToDelete);

            Context.Structures.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStructureDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnStructureGet(Models.AppDb.Structure item);

        public async Task<Models.AppDb.Structure> GetStructureById(int? id)
        {
            var items = Context.Structures
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnStructureGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AppDb.Structure> CancelStructureChanges(Models.AppDb.Structure item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStructureUpdated(Models.AppDb.Structure item);
        partial void OnAfterStructureUpdated(Models.AppDb.Structure item);

        public async Task<Models.AppDb.Structure> UpdateStructure(int? id, Models.AppDb.Structure structure)
        {
            OnStructureUpdated(structure);

            var itemToUpdate = Context.Structures
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(structure);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterStructureUpdated(structure);

            return structure;
        }
    }
}

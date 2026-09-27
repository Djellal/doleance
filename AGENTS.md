# AGENTS.md

## What this is

Single-project ASP.NET Core **Blazor Server** app (`net10.0`) — a "doléances" (complaints) and
appointment-request system for Université Ferhat Abbas Sétif 1. UI is **Radzen.Blazor 4.34.4**;
nearly every component is Radzen (`RadzenDataGrid`, `RadzenForm`, `RadzenDialog`, …).

It started as a Radzen Blazor template scaffold, which is why the code is split generated/hand-written.
Treat "generated" files as *owned by this repo now* — the Radzen generator is not re-run here.

No test project, no CI, no lint/format config.

## Build & run

```bash
dotnet build Doleance.csproj     # verified working
dotnet run --no-launch-profile   # verified working, no env vars needed
```

Both work as-is. `appsettings.json` points at `Data Source=app.db;` (relative), so the checked-in
`app.db` is picked up from the working directory. There is no `appsettings.Development.json`.

Local SDK is **10.0.112** with only the 10.0.12 runtime, matching `net10.0`. Don't add
`DOTNET_ROLL_FORWARD` or connection-string overrides — the earlier versions of this file said to
and that guidance is obsolete.

Pre-existing warnings, **not** caused by your change:
`NU1903` (`System.Linq.Dynamic.Core` 1.3.7, pulled in transitively by Radzen), `CS8981` ×2
(the `addstrid` migration class is lower-cased), `ASP0019` (`Controllers/ReportController.cs:239`
uses `IDictionary.Add` on headers — duplicate key throws `ArgumentException`).

URL is **http://localhost:5020**, not 5000. `Program.cs:27` calls `webBuilder.UseUrls(...)`, which
overrides `Properties/launchSettings.json`. Don't chase the launchSettings port.

`ASPNETCORE_ENVIRONMENT=Development` enables the developer exception page **and** the dev backdoor
below. All launch profiles set it, so you get it by default.

## Stale configs that will bite you

- **`.vscode/launch.json` still points at `bin/Debug/net6.0/Doleance.dll`.** The build now outputs
  `net10.0`, so "start debugging" from VS Code fails. Update the path if you touch that file.
- **`Dockerfile` pins `mcr.microsoft.com/dotnet/sdk:6.0` but the project targets `net10.0`,** so
  `dotnet publish` inside the image cannot work. The Dockerfile is unmaintained — the real deploy is
  IIS/`web.config` (`processPath="dotnet"`, `arguments=".\Doleance.dll"`). Fix the base image before
  relying on it.
- **There is no `.gitignore`, and `bin/`, `obj/`, `app.db`, and `.vs/` are already tracked** (~617
  artifact files in the initial commit). A `git status` after building will show churn on binaries.
  Only stage files you actually edited.

## Architecture notes that aren't obvious from filenames

**Two DbContexts on one SQLite file.** `AppDbContext` (domain: Doleanctabs, Rendezvous, Structures,
Appartenances, Qualites) and `ApplicationIdentityDbContext` (ASP.NET Identity). Both are registered
against the same `AppDbConnection` in `Startup.cs:61-77`.

**`app.db` is a checked-in binary artifact and the schema source of truth.**
`Data/Migrations` only covers the Identity schema — `CreateIdentitySchema` plus
`20230129120343_addstrid` (adds `StructId` to `AspNetUsers`). The five AppDb tables are **not** in
any migration, and `identityDbContext.Database.Migrate()` is commented out (`Startup.cs:129`).
Changing an AppDb entity's shape means hand-editing `app.db` (no `sqlite3` CLI is installed here —
use `dotnet ef` or a scratch tool); `dotnet ef migrations add` will not pick up AppDb entities.

**Generated vs. hand-written code.**

| File | Status |
| --- | --- |
| `*.razor.designer.cs` | generator output — **already hand-edited in places** |
| `Pages/*.razor`, `*.razor.cs` | hand-written |
| `*.Custom.cs` | hand-written — **put new logic here** |
| `AppDbService.cs`, `AppDbContext.cs`, `SecurityService.cs` | generated, partly hand-edited |
| `AppDbService.Custom.cs`, `Startup.Custom.cs`, `Data/AppDbContext.Custom.cs`, `Models/ApplicationUser.Custom.cs` | hand-written |

The intended extension points are the `partial void` hooks declared in the generated files
(`OnDoleanctabCreated`, `OnAfterDoleanctabUpdated`, `OnRendezvousRead(ref IQueryable<T>)`, …).
Implement them in the matching `.Custom.cs` file. Note the file is spelled
`SecurityService.Custon.cs` (typo is in the real path).

**`*.razor.designer.cs` holds all the page logic** — `OnInitializedAsync`, `Load()`, and every event
handler such as `Form0Submit`. Custom methods added to `*.razor.cs` are only reachable because the
generated handlers were edited to call them (see `AddDoleanctab.razor.designer.cs:140` calling
`SendDoleanceMail()` in `AddDoleanctab.razor.cs`). Editing a designer file is the norm here, not an
error — just be deliberate.

**Auth is layered — check both mechanisms before adding a page.**
1. `@attribute [Authorize]` / `[Authorize(Roles=...)]` on 17 pages in `Pages/*.razor`, enforced by
   `AuthorizeRouteView` in `App.razor`. Role strings in use: bare `[Authorize]` (10),
   `"Authenticated, admin"` (5), `"Authenticated, admin, structadmin"` (2). Note `Authenticated` is
   a *literal* role name, not one of `Constants` — a latent oddity, so don't "fix" it casually.
2. An imperative guard: 20 designer files call `Security.InitializeAsync(AuthenticationStateProvider)`
   in `OnInitializedAsync`, and role gating uses `Security.IsInRole(...)` both there and to
   conditionally render controls in the `.razor` markup.

Real roles are `admin`, `structadmin`, `user` (`Constants.cs`), created at every startup by
`EnsureRolesAsync` (`Startup.Custom.cs:75`, called synchronously via `.Wait()` at `Startup.cs:131`).

**Dev backdoor.** `SecurityService.cs:73`: when `env.EnvironmentName == "Development"`, *any*
authenticated principal named `admin` is replaced by an in-memory `ApplicationUser` with a **null
`Id`**. Code that depends on `Security.User.Id` (e.g. `AddDoleanctab.razor.designer.cs:115` writing
`doleanctab.userid`) will misbehave under that path. Don't mistake it for a real user.

**Sequence numbers are `Count() + 1`** — `AppDbService.Custom.cs:20-30` (`GetNewNumInscription`,
`GetNewNumRdv`). Not transactional, collides under concurrency. Preserve the format (`D-0001`,
`RDV-0001`) if you touch them.

**Exports are navigation-based.** `AppDbService.Export*ToExcel/CSV` don't return a file — they
`NavigationManager.NavigateTo("export/appdb/<entity>/excel(fileName='...')", true)`. The URL string
must keep matching the `[HttpGet]` routes in `Controllers/ExportAppDbController.cs`; the handling
(`ToExcel`/`ToCSV` + `ApplyQuery`) lives in `Controllers/ExportController.cs`.

**SSRS reports are reverse-proxied.** `Controllers/ReportController.cs` exposes `/__ssrsreport` and
`/ssrsproxy/{*url}`, which parse the incoming URL to recover the upstream SSRS host and forward the
request. It rewrites self-referencing URLs to bounce through the proxy — brittle; report URLs must
keep the `/ReportServer` or `/Reports` path segment or `IndexOf` returns -1 and it throws.

**Culture is forced to `fr-FR`** with `.` as the decimal separator (`Startup.Custom.cs:38-42`).
Don't switch to invariant-culture assumptions for numeric parsing/formatting.

**All user-facing text is French** — UI labels, validation messages, and email bodies. Match it.

## Operational gotchas

- **Mail works only from inside the university network.** `Startup.Custom.cs:62` binds
  `services.Configure<MailSettings>(Configuration.GetSection("MailSettings"))`, but
  `appsettings.json` has **no `MailSettings` section**, so the object is empty and
  `MailService.SendEmailAsync` (the MailKit path) will throw on `Connect`. The path actually used,
  `SendEmail`, hardcodes the SMTP host and credentials in `Services/MailService.cs` — do not copy
  those into new code, docs, or logs; if you touch mail, move it into the `MailSettings` config
  section.
- **`radzen_*.log` files in the repo root are IIS stdout logs**, produced by
  `stdoutLogFile=".\radzen"` in `web.config`. They are runtime artifacts, not source.
- `Doleance.csproj` sets `NoWarn` for CS0168, CS1998, BL9993, CS0649, CS0436 — unused fields and
  `async`-without-`await` won't be flagged, so don't rely on the compiler to catch them.
- `EnsureRolesAsync` runs as a blocking `.Wait()` during `Configure`, so any DB problem surfaces as a
  startup crash, not a request-time error. `SQLite Error 14: 'unable to open database file'` almost
  always means the working directory isn't where `app.db` lives.

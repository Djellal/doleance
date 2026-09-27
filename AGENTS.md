# AGENTS.md

## What this is

Single-project ASP.NET Core **Blazor Server** app (`net6.0`) — a "doléances" (complaints) and
appointment-request system for Université Ferhat Abbas Sétif 1. UI is **Radzen.Blazor 4.23.8**;
nearly every component is Radzen (`RadzenDataGrid`, `RadzenForm`, `RadzenDialog`, …).

No solution file, no test project, no CI config, no lint/format config, **not a git repo**.

## Build & run

```powershell
dotnet build Doleance.csproj          # verified working
```

Build succeeds on SDK 10.0.401 even though the project targets `net6.0` (targeting pack comes
from NuGet). Expect these pre-existing warnings — they are not caused by your change:
`NETSDK1138` (net6.0 EOL), `NU1902`/`NU1903` (MailKit 3.3.0, MimeKit 3.3.0, SqlClient 2.1.4 advisories).

Running locally **requires two env vars** or the app dies at startup:

```powershell
$env:DOTNET_ROLL_FORWARD="LatestMajor"
$env:ConnectionStrings__AppDbConnection="Data Source=app.db"
dotnet run --no-launch-profile
```

- `DOTNET_ROLL_FORWARD=LatestMajor` — no .NET 6 runtime is installed (only 8/9/10). Without it
  `dotnet run` exits with "You must install or update .NET to run this application".
- `ConnectionStrings__AppDbConnection` — `appsettings.json` hardcodes the **Linux deploy path**
  `Data Source=/opt/doleanceapp/app.db`. On Windows that does not exist, and startup crashes with
  `SQLite Error 14: 'unable to open database file'` from `EnsureRolesAsync` (`Startup.cs:131`).
  The csproj copies the checked-in `app.db` next to the binary, so `Data Source=app.db` is the
  intended local value.

URL is **http://localhost:5020**, not 5000. `Program.cs:27` calls `webBuilder.UseUrls(...)`, which
overrides `Properties/launchSettings.json`. Don't chase the launchSettings port.

`ASPNETCORE_ENVIRONMENT=Development` changes behaviour (see dev backdoor below) and enables the
developer exception page. Use `--no-launch-profile` only if you don't want Development.

## Architecture notes that aren't obvious from filenames

**Two DbContexts on one SQLite file.** `AppDbContext` (domain: Doleanctabs, Rendezvous, Structures,
Appartenances, Qualites) and `ApplicationIdentityDbContext` (ASP.NET Identity). Both are registered
against the same `AppDbConnection` in `Startup.cs:61-77`.

**`app.db` is a checked-in binary artifact and the schema source of truth.**
`Data/Migrations` only covers the Identity schema (AspNet* tables + `ApplicationUser` extras) — the
five AppDb tables are **not** in any migration, and `identityDbContext.Database.Migrate()` is
commented out (`Startup.cs:129`). Changing an AppDb entity's shape means hand-editing `app.db`
(or adding a fresh `EnsureCreated`/migration yourself); `dotnet ef migrations add` will not pick
up AppDb entities.

**Generated vs. hand-written code.** The app was scaffolded from the Radzen Blazor template, which
splits each type into a generated file plus a `*.Custom.cs` partial:

| File | Status |
| --- | --- |
| `*.razor.designer.cs` | generator output — **already hand-edited in places** |
| `Pages/*.razor`, `*.razor.cs` | hand-written |
| `*.Custom.cs` | hand-written — **put new logic here** |
| `AppDbService.cs`, `AppDbContext.cs`, `SecurityService.cs` | generated, partly hand-edited |
| `AppDbService.Custom.cs`, `Startup.Custom.cs`, `Models/ApplicationUser.Custom.cs` | hand-written |

The intended extension points are the `partial void` hooks already declared in the generated files:
`OnDoleanctabCreated`, `OnAfterDoleanctabUpdated`, `OnRendezvousRead(ref IQueryable<T>)`, etc.
Implement them in the matching `.Custom.cs` file. Note the file is spelled
`SecurityService.Custon.cs` (typo is in the real path).

**`*.razor.designer.cs` holds all the page logic** — `OnInitializedAsync`, `Load()`, and every event
handler such as `Form0Submit`. Custom methods added to `*.razor.cs` are only reachable because the
generated handlers were edited to call them (see `AddDoleanctab.razor.designer.cs:140` calling
`SendDoleanceMail()` in `AddDoleanctab.razor.cs`). Editing a designer file is the norm here, not an
error — just be deliberate.

**No `[Authorize]` on Blazor pages.** Auth is enforced imperatively: each page's generated
`OnInitializedAsync` calls `Security.InitializeAsync(...)` and redirects to `Login`; role gating
uses `Security.IsInRole(...)` inside `OnInitializedAsync` and in `Shared/MainLayout.razor` menu
items. Roles are `admin`, `structadmin`, `user` (`Constants.cs`), created at every startup by
`EnsureRolesAsync` (`Startup.Custom.cs:75`). If you add a page, replicate that guard — nothing
will block an unguarded route for you.

**Dev backdoor.** `SecurityService.cs:73-76`: in `Development`, *any* authenticated principal named
`admin` is replaced by an in-memory `ApplicationUser` with a **null `Id`**. Code that depends on
`Security.User.Id` (e.g. `AddDoleanctab.razor.designer.cs:115` writing `doleanctab.userid`) will
misbehave under that path. Don't mistake it for a real user.

**Sequence numbers are `Count() + 1`** — `AppDbService.Custom.cs` (`GetNewNumInscription`,
`GetNewNumRdv`). Not transactional, collides under concurrency. Preserve the format (`D-0001`,
`RDV-0001`) if you touch them.

**Exports are navigation-based.** `AppDbService.Export*ToExcel/CSV` don't return a file — they
`NavigationManager.NavigateTo("export/appdb/<entity>/excel(fileName='...')", true)`. The URL string
must keep matching the `[HttpGet]` routes in `Controllers/ExportAppDbController.cs`; the handling
(`ToExcel`/`ToCSV` + `ApplyQuery`) lives in `Controllers/ExportController.cs`.

**Culture is forced to `fr-FR`** with `.` as the decimal separator (`Startup.Custom.cs:38-49`).
Don't switch to invariant-culture assumptions for numeric parsing/formatting.

**All user-facing text is French** — UI labels, validation messages, and email bodies. Match it.

## Operational gotchas

- **Mail will not work from outside the university network.** `appsettings.json` has **no
  `MailSettings` section**, so the bound `MailSettings` object is empty and `MailService.SendEmailAsync`
  (the MailKit path) will throw. The path actually used, `SendEmail`, hardcodes the SMTP host and
  credentials in `Services/MailService.cs` — do not copy that into new code, docs, or logs; if you
  touch mail, move it to the `MailSettings` config section.
- **`radzen_*.log` files in the repo root are IIS stdout logs**, produced by
  `stdoutLogFile=".\radzen"` in `web.config`. They are runtime artifacts, not source.
- **Deploy** is the `Dockerfile`: SDK 6.0 image, `dotnet publish -c Release -o out`, then
  `dotnet Doleance.dll` on `ASPNETCORE_URLS=http://*:5000` with the db mounted at
  `/opt/doleanceapp/app.db`. `Dockerfile` uses `COPY . /app` with no `.dockerignore`, so `app.db`,
  `bin/`, and `obj/` are all copied into the image.
- `Doleance.csproj` sets `NoWarn` for CS0168, CS1998, BL9993, CS0649, CS0436 — unused fields and
  `async`-without-`await` won't be flagged, so don't rely on the compiler to catch them.

# eShopLegacyMVC

This legacy MVC application is build with ASP.NET 4.7.2. Follow the steps below to port this application to .NET5.0.

## Upgrade

### Portability analyzers

Optionally, you can use of the following portability analyzers before you start the upgrade. These tools will analyze assemblies and provides a detailed report about the impact of porting application to specified .NET platform(s).

- [Porting Assistant for .NET](https://docs.aws.amazon.com/portingassistant/latest/userguide/porting-assistant-install.html)
- [Visual Studio portability analyzer](https://docs.microsoft.com/en-us/dotnet/standard/analyzers/portability-analyzer)

### try-convert

For non-web application, you can use the `try-convert` tool. This tool will try to migrate your .NET Framework project. Find more info [here](https://github.com/dotnet/try-convert). Unfortunately, it doesn't work for web application like the eShopLegacyMVC project.

### Migrate `packages.config`

> The migration tool doesn't support ASP.NET projects. However, there is a [work around](https://github.com/NuGet/docs.microsoft.com-nuget/issues/860#issuecomment-409207305). Update the `.csproj` like is mentioned in the comment before continue.

- Migrate `packages.config` to `PackageReference`. Follow the steps in [this tutorial](https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference).

> If it doesn't work for you find the upgraded `.csproj` [here](docs/empty-csproj.md).

#### Manually modifications

The migrate tool is of course not a magically tool that will do all work. There will still be things you'll need to do manually. Remove the following legacy packages (either not compatible with NET5.0 or just legacy packages).

**Remove packages from `.csproj`**

- `WebGrease`
- `Microsoft.Web.Infrastructure`
- `Antlr`
- `Microsoft.ApplicationInsights.Agent.Intercept`
- All `Microsoft.AspNet.*` packages
- `Microsoft.CodeDom.Providers.DotNetCompilerPlatform`
- `Microsoft.Net.Compilers`
- All `Autofac.*` packages. We can use Microsoft's built-in dependency injection
- `bootstrap`
- All `jQuery.*` packages
- `popper.js`
- `response`
- `log4net`

**Replace**

- `EntityFramework` with `Microsoft.EntityFrameworkCore`

**Upgrade versions**

- `System.Diagnostics.DiagnosticSource` and `System.Diagnostics.PerformanceCounter` to 5.0.0
- `System.Diagnostics.DiagnosticSource` to 5.0.1
- `System.Buffers` to 4.5.1
- `System.Runtime.CompilerServices.Unsafe` to 5.0.0

**Install**

- `Microsoft.AspNetCore.Mvc.Core` 2.2.5
- `Microsoft.EntityFrameworkCore.SqlServer`

**Clean up `.csproj`**

- Clean up the `.csproj` most things you'll not need anymore.
- Update the [target framework](https://docs.microsoft.com/en-us/aspnet/core/migration/31-to-50?view=aspnetcore-5.0&tabs=visual-studio#update-the-target-framework)

> Find upgraded `.csproj` [here](docs/empty-csproj.md).

### Migrate Global.asax

- Create `Program.cs`
- Create `Startup.cs`

Follow steps [here](https://docs.microsoft.com/en-us/aspnet/core/migration/proper-to-2x/?view=aspnetcore-5.0#globalasax-file-replacement)

### Remove files

- `AssemblyInfo.cs`
- `web.config`
- `Global.asax`
- `ApplicationInsights.config`

### Create wwwroot folder

- Move Content to wwwroot/css/
- Move Scripts to wwwroot/scripts/
- Move Pics to wwwroot/pics/
- Move fonts to wwwroot/fonts/
- Move Images to wwwroot/images/
- Move favicon.ico to wwwroot

Follow steps [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-5.0).

### Configuration

Read about configurations [here](https://docs.microsoft.com/en-us/aspnet/core/migration/configuration?view=aspnetcore-5.0).

- Create `appsettings.json`
- Create `ConnectionStrings` with `DefaultConnection`. Get connection string from `web.config`
- Move settings (`UseMockData` and `UseCustomizationData`) to `appsettings.json` Create parent element `DataSettings`
- Learn how to work with options [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0)

### App_Start migration

- Remove `WebApiConfig.cs`
- `BundleConfig.cs`
  - Install `BuildBundlerMinifier` NuGet package
  - Create `bundleconfig.js` in root
  - Create one bundle for styles and one for scripts
  - Follow tutorial [here](https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-5.0)
  - Remove `BundleConfig.cs`
- Remove `FilterConfig.cs`
- Remove `RouteConfig.cs`
- Update script and style references in MVC views

### MVC Controllers

- Update namespaces in controllers
- `HttpNotFound` => `NotFound`
- `new HttpStatusCodeResult(HttpStatusCode.BadRequest);` => `BadRequest()`
- Remove the `Include` property
- `this.Request.Url.Scheme` to `this.Request.Scheme`
- Inject `IWebHostEnvironment` in controller and use instead of `Server.MapPath`

#### Action/ Partial to Component

Learn about components [here](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-5.0)

- Change CatalogStatisticsController to a ViewComponent. Call component in razor view (`Views/Catalog/Index.cshtml`)
- Change `Html.Partial` to `Html.PartialAsync`

### Migrate ASP.NET Web API to ASP.NET Core

Read more [here](https://docs.microsoft.com/en-us/aspnet/core/migration/webapi?view=aspnetcore-5.0)

### EntityFramework

- CatalogDBContext and CatalogService
- update namespaces

### Work with sessions

- `services.AddSession` call in `Startup.cs`
- `app.UseSession` call in `Startup.cs`
- Use `Context.Session` instead of `HttpContext.Current.Session`

### Dependency injection

- Move `ApplicationModule registrations` to `Startup.cs`
- Remove ApplicationModule class

Read more [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0)

### Logging

We will use `Microsoft.Extensions.Logging` abstraction and install `log4net` provider.

- Install `Microsoft.Extensions.Logging.Log4Net.AspNetCore` package
- Follow steps [here](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore)
- Update CatalogController and inject `ILogger<CatalogController> logger` instead of using `log4net` directly

Read more about `Microsoft.Extensions.Logging` [here](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-5.0)

### Add migration

If all compile errors have been fixed add a migration to set up the database.

- `dotnet ef migrations add InitialCreate`

### Run!

- verify if everything work as expected

### Blazor!

Follow the Blazor [tutorial](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro).

- TODO: more documentation here
- Use Blazor instead of MVC

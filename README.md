# dotNETCoreRESTAPI
Reference guide for REST API in Microsoft .NET Core.



## Requirements

**.NET Core Framework**

https://dotnet.microsoft.com/download

**Visual Studio Code**

https://code.visualstudio.com/


## VS Code Extensions

**C# for Visual Studio Code (powered by OmniSharp)**

https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp

**C# Extensions**

https://marketplace.visualstudio.com/items?itemName=kreativ-software.csharpextensions

**C# Snippets**

https://marketplace.visualstudio.com/items?itemName=jorgeserrano.vscode-csharp-snippets

**C# Full namespace autocompletion**

https://marketplace.visualstudio.com/items?itemName=adrianwilczynski.namespace

**SQLite Database Viewer**

https://marketplace.visualstudio.com/items?itemName=qwtel.sqlite-viewer


## Documentation

**Microsoft dotNET Core**

https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6

**Migrations**

https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

**Microsoft dotNET Core Code Generator**

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-6.0


## Project Creation Scripts

**Create new solution**

    dotnet new sln -n dotNETCoreRESTAPI

**Create new webapi project**

    dotnet new webapi -n dotNETCoreRESTAPI

**Add project to solution**

    dotnet sln dotNETCoreRESTAPI.sln add dotNETCoreRESTAPI/dotNETCoreRESTAPI.csproj


## Project packages

**EntityFrameworkCore**

    dotnet add package Microsoft.EntityFrameworkCore --version 6.0.5

**EntityFrameworkCore Identity**

    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0.5

**EntityFrameworkCore Identity UI**

    dotnet add package Microsoft.AspNetCore.Identity.UI --version 6.0.5

**EntityFrameworkCore Design**

    dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.5

**EntityFrameworkCore Tools**

    dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.5

**Code Generation MVC (Scaffold)**

    dotnet add package Microsoft.VisualStudio.Web.CodeGenerators.Mvc --version 6.0.5

**Code Generation Design (Scaffold)**

    dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 6.0.5

**EFCore SQL Server**

    dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.5

**SQLite**

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0.5

**NewtonsoftJson**

    dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 3.0.0


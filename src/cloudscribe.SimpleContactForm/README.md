# cloudscribe.SimpleContactForm

A simple, reusable contact form for ASP.NET Core applications. Easily integrate a contact form into your site and handle submissions with extensible options.

## Features
- Plug-and-play contact form for ASP.NET Core
- Customizable fields and validation
- Extensible for custom logic
- Supports dependency injection

## Usage
1. Install the NuGet package:
   ```shell
   dotnet add package cloudscribe.SimpleContactForm
   ```
2. Register the service in your `Startup.cs` or `Program.cs`:
   ```csharp
   services.AddCloudscribeSimpleContactForm();
   ```
3. Add the form to your Razor view:
   ```csharp
   @await Component.InvokeAsync("SimpleContactForm")
   ```
4. Configure options as needed in your `appsettings.json`.

## License
Licensed under the Apache License, Version 2.0. See [LICENSE](https://www.apache.org/licenses/LICENSE-2.0) for details.

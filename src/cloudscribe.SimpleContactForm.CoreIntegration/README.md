# cloudscribe.SimpleContactForm.CoreIntegration

Integration library for using cloudscribe.SimpleContactForm with cloudscribe.Core. Provides seamless wiring and additional features for cloudscribe-based sites.

## Features
- Integrates SimpleContactForm with cloudscribe.Core
- Enables contact form on cloudscribe multi-tenant sites
- Extensible for custom scenarios

## Usage
1. Install the NuGet package:
   ```shell
   dotnet add package cloudscribe.SimpleContactForm.CoreIntegration
   ```
2. Register the integration in your `Startup.cs` or `Program.cs`:
   ```csharp
   services.AddCloudscribeSimpleContactFormCoreIntegration();
   ```
3. Use the form as part of your cloudscribe.Core site.

## License
Licensed under the Apache License, Version 2.0. See [LICENSE](https://www.apache.org/licenses/LICENSE-2.0) for details.

# WashingCore
哈尔滨市博鑫顺清洁服务公司在线服务

This repo contains the library implementation (called "CoreFX") for .NET Core. It includes System.Collections, System.IO, System.Xml, and many other components.
The corresponding [.NET Core Runtime repo](https://github.com/dotnet/coreclr) (called "CoreCLR") contains the runtime implementation for .NET Core. It includes RyuJIT, the .NET GC, and many other components.
Runtime-specific library code ([mscorlib](https://github.com/dotnet/coreclr/tree/master/src/mscorlib)) lives in the CoreCLR repo. It needs to be built and versioned in tandem with the runtime. The rest of CoreFX is agnostic of runtime-implementation and can be run on any compatible .NET runtime (e.g. [CoreRT](https://github.com/dotnet/corert)).



## .NET Core

Official Starting Page: http://dotnet.github.io

* [How to use .NET Core](https://github.com/dotnet/core/#get-started) (with VS, VS Code, command-line CLI)
  * [Install official releases](https://www.microsoft.com/net/core)
  * [Documentation](https://docs.microsoft.com/en-us/dotnet) (Get Started, Tutorials, Porting from .NET Framework, API reference, ...)
    * [Deploying apps](https://docs.microsoft.com/en-us/dotnet/articles/core/preview3/deploying)
  * [Supported OS versions](https://github.com/dotnet/core/blob/master/roadmap.md#technology-roadmaps)
* [Roadmap](https://github.com/dotnet/core/blob/master/roadmap.md)
* [Releases](https://github.com/dotnet/core/tree/master/release-notes)
* [Bringing more APIs to .NET Core](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/porting.md) (and why some APIs will be left out)



## How to Engage, Contribute and Provide Feedback

Some of the best ways to contribute are to try things out, file bugs, join in design conversations, and fix issues.

* [Dogfooding daily builds](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/dogfooding.md)
* If you have a question or idea, [file a new issue](https://github.com/dotnet/corefx/issues/new).

If you are having issues with the "full" .NET Framework (also called "Desktop"), the best way to file a bug is the [Report a Problem](https://aka.ms/vs-rap) tool, which is integrated with the [VS Developer Community Portal](https://developercommunity.visualstudio.com/); or through [Product Support](https://support.microsoft.com/en-us/contactus?ws=support) if you have a contract.

### Issue Guide

This section is **in progress** here: [New contributor Docs - Issues](https://github.com/dotnet/corefx/wiki/New-contributor-Docs#issue-guide) (feel free to make it better - it's easy-to-edit wiki with RW permissions to everyone!)

Each issue area has one or more Microsoft owners, who are [listed here](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/issue-guide.md).

### Contributing Guide

This section is **in progress** here: [New contributor Docs - Contributing](https://github.com/dotnet/corefx/wiki/New-contributor-Docs#contributing-guide) (feel free to make it better - it's easy-to-edit wiki with RW permissions to everyone!) 

### Useful Links

* [.NET Core source index](https://source.dot.net) / [.NET Framework source index](https://referencesource.microsoft.com)
* [API Reference docs](https://docs.microsoft.com/en-us/dotnet/core/api)
* [.NET API Catalog](http://apisof.net) (incl. APIs from daily builds and API usage info)



### Reporting security issues and security bugs

Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC) <secure@microsoft.com>. You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the MSRC PGP key, can be found in the [Security TechCenter](https://technet.microsoft.com/en-us/security/ff852094.aspx).

Also see info about related [Microsoft .NET Core and ASP.NET Core Bug Bounty Program](https://technet.microsoft.com/en-us/mt764065.aspx).

## License

.NET Core (including the corefx repo) is licensed under the [MIT license](LICENSE.TXT).



## .NET Foundation

.NET Core is a [.NET Foundation](http://www.dotnetfoundation.org/projects) project.

There are many .NET related projects on GitHub.

- [.NET home repo](https://github.com/Microsoft/dotnet) - links to 100s of .NET projects, from Microsoft and the community.
- [ASP.NET Core home](https://github.com/aspnet/home) - the best place to start learning about ASP.NET Core.

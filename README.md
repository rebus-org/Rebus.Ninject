# Rebus.Ninject

[![install from nuget](https://img.shields.io/nuget/v/Rebus.Ninject.svg?style=flat-square)](https://www.nuget.org/packages/Rebus.Ninject)

Provides a Ninject container adapter for [Rebus](https://github.com/rebus-org/Rebus).

![](https://raw.githubusercontent.com/rebus-org/Rebus/master/artwork/little_rebusbus2_copy-200x200.png)

---

_Attention users of ASP.NET MVC5_: You are probably planning on using the `Ninject.MVC5` package to integration your Ninject container with your web application, but unfortunately (at least at the time of writing) the package depends on version 3 of Ninject, where Rebus.Ninject depends on the upcoming version 4 (because it supports .NET Core).

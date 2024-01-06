# run-grasshopper-demo

This repository aims to cover all possible ways of running a headless Grasshopper instance in various configurations:

- different Rhino versions
- different .NET runtimes
- different programming languages (coming soon...)

## Motivation

At the time of writing (6 January 2024), the barrier to entry for starting developing Rhino/Grasshopper applications is quite high. The official documentation is not very clear and the examples are not very well documented.

Coming from a TypeScript background, I found that the .NET ecosystem is quite different and the learning curve is quite steep e.g. working with Visual Studio, assembly references, .NET runtimes, etc.

Taking into consideration today's advances in AI (specifically the text-to-shape research), I believe that this sort of effort can serve as a good start for developers and researchers with no prior experience in the .NET ecosystem to start developing their own applications.

This repository is my take on (hopefully) making this process a little bit easier.

## List of available demos

- .NET

  - net48

    - [Rhino 7.0 with Rhino.Inside](./rhino-7-with-rhino-inside-net48/README.md)
    - [Rhino 7.0 with "manually" implemented Rhino.Inside](./rhino-7-custom-rhino-inside-net48/README.md)

  - net7.0

    - [Rhino 8.0 with Rhino.Inside](./rhino-8-with-rhino-inside-net7/README.md) - **WIP (as of 2024.01.06)**
    - [Rhino 8.0 with "manually" implemented Rhino.Inside](./rhino-8-custom-rhino-inside-net7/README.md) - **Recommended (as of 2024.01.06)**

- Python
  - Coming soon...

## Resources

- Official `SampleRunGrasshopper` example for Rhino 7: https://github.com/mcneel/rhino-developer-samples/blob/7/rhino.inside/dotnet/SampleRunGrasshopper
- Official `SampleRunGrasshopper` example for Rhino 8: https://github.com/mcneel/rhino-developer-samples/blob/8/rhino.inside/dotnet/SampleRunGrasshopper
- Official custom `RhinoInside.Resolver` for Rhino 7: https://github.com/mcneel/rhino.inside/blob/will/rhinoinside-v7/DotNet/RhinoInside
- Official custom `RhinoInside.Resolver` for Rhino 8: https://github.com/mcneel/compute.rhino3d/blob/8.x/src/compute.geometry/Resolver.cs (thanks to [this comment](https://discourse.mcneel.com/t/rhino-inside-net-core-7-0/166059/3?u=gabriel15) from Steve Baer)

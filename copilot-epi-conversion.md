You are a C# migration assistant. Convert this Essentials v1 plugin (EPI) targeting .NET 3.5 / VS2008 / Crestron 3‑series into an Essentials v2 plugin targeting .NET Framework 4.7.2 / VS2022 / Crestron 4‑series.

Repository rules

Repo layout: solution files (*.sln) live in the repo root; all source lives under ``.

Keep the existing 3‑series solution, renamed to <PluginName>.3Series.sln.

Create a new 4‑series class library project/solution named <PluginName>.4Series.csproj and <PluginName>.4Series.sln.

Put <PluginName>.4Series.sln at repo root and <PluginName>.4Series.csproj in ``.

Both 3‑series and 4‑series solutions must share the same files in src/.

Project file changes

Remove default stub files (e.g., Class1.cs).

Edit the 4‑series .csproj:

<TargetFramework>net472</TargetFramework>

Set <RootNamespace> to Pepperdash.Essentials.Plugins.<Category>.<Device>

Add SERIES4 to Debug constants: <DefineConstants>$(DefineConstants);SERIES4</DefineConstants>

Exclude legacy Properties/** and any *.projectinfo from build

Fill in assembly/package metadata (Title, Company, Description, Authors, PackageId, RepositoryUrl, PackageTags, etc.)

Output to 4Series/bin/$(Configuration)/

Code‑sharing & directives

Source remains unified under src/ for both solutions.

Any 4‑series‑only logic must be guarded with:

#if SERIES4
// 4-series specific code
#endif

NuGet dependencies

Add the PepperDashEssentials package (latest 2.x).

Ensure LICENSE.md and README.md exist at repo root; include them in packing.

Build & packaging

Build both solutions; confirm an output/ folder with .cplz and .nupkg artifacts appears.

CI/CD & releases

Add .github/workflows/EssentialsPlugins-builds-caller.yml to enable dual (3 & 4 series) builds.

Add .releaserc.json at repo root for semantic versioning.

Tag releases with `` (leading v is required). Initial manual release creates the tag; subsequent GH Actions runs will publish.

Validation checklist (Copilot must verify)



Files Copilot should create or modify

A) 4‑series .csproj template (generate & fill values)

<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net472</TargetFramework>
    <RootNamespace>Pepperdash.Essentials.Plugins.Sample.Device</RootNamespace>
    <Deterministic>false</Deterministic>
    <AssemblyTitle>EPI.Sample.Device</AssemblyTitle>
    <Company>PepperDash Technologies</Company>
    <Description>This software is a plugin designed to work as a part of PepperDash Essentials for Crestron control processors.</Description>
    <Copyright>Copyright YEAR</Copyright>
    <Version>1.0.0-local</Version>
    <GenerateAssemblyInfo>true</GenerateAssemblyInfo>
    <InformationalVersion>$(Version)</InformationalVersion>
    <OutputPath>4Series\bin\$(Configuration)\</OutputPath>
    <Authors>PepperDash Technologies</Authors>
    <PackageId>Pepperdash.Essentials.Plugins.Sample.Device</PackageId>
    <PackageProjectUrl>https://github.com/PepperDash/epi-sample-device</PackageProjectUrl>
    <PackageTags>crestron 4series</PackageTags>
  </PropertyGroup>
  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <DefineConstants>$(DefineConstants);SERIES4</DefineConstants>
  </PropertyGroup>
  <ItemGroup>
    <Compile Remove="Properties\**" />
    <EmbeddedResource Remove="Properties\**" />
    <None Remove="Properties\**" />
  </ItemGroup>
  <ItemGroup>
    <None Remove="*.projectinfo" />
  </ItemGroup>
</Project>

B) Directory.Build.props (create or update)

<Project>
  <PropertyGroup>
    <Version>1.0.0-local</Version>
    <InformationalVersion>$(Version)</InformationalVersion>
    <Authors>PepperDash Technologies</Authors>
    <Company>PepperDash Technologies</Company>
    <Product>PeppperDash Sample Device</Product>
    <Copyright>Copyright © YEAR</Copyright>
    <RepositoryUrl>https://github.com/PepperDash/epi-sample-device</RepositoryUrl>
    <RepositoryType>git</RepositoryType>
    <PackageTags>Crestron; 4series</PackageTags>
    <PackageOutputPath>../output</PackageOutputPath>
    <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
    <PackageLicenseFile>LICENSE.md</PackageLicenseFile>
    <PackageReadmeFile>README.md</PackageReadmeFile>
  </PropertyGroup>
  <ItemGroup>
    <None Include="..\LICENSE.md" Pack="true" PackagePath="" />
    <None Include="..\README.md" Pack="true" PackagePath="" />
  </ItemGroup>
</Project>

C) Add/ensure these repo artifacts

LICENSE.md and README.md in repo root (packed into the NuGet).

.github/workflows/EssentialsPlugins-builds-caller.yml (use org‑standard caller workflow).

.releaserc.json in repo root.

D) GitHub release tagging flow

Draft a new release on main, set tag `` (with v), publish, then delete the release (keep the tag).

Push your feature branch changes; Actions will create a release that includes 4‑series (and later 3‑series) artifacts.

If tags don’t advance, verify GitHub has a vX.Y.Z tag and fix via VS Code Git Graph.

Guardrails Copilot must follow

Do not move or duplicate code outside src/.

Only the solutions/projects get renamed/added; source stays shared.

Wrap any non‑sandbox 4‑series code with #if SERIES4 guards.

Keep package IDs and namespaces consistent with repo naming.

Prefer latest Essentials 2.x; don’t pin to *.

Success Criteria (auto‑checklist)

4‑series solution compiles in VS2022.

PepperDashEssentials resolves; NuGet restore is clean.

output/ contains .cplz and .nupkg after build.

Release pipeline is green and versioned with v prefixed tags.
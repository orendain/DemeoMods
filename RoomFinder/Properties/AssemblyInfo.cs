﻿using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using RoomFinder;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("RoomFinder")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Orendain")]
[assembly: AssemblyProduct("RoomFinder")]
[assembly: AssemblyCopyright("Copyright ©  2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5D106A6F-2A66-4731-877A-3C315F0CB18C")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(BuildVersion.MajorMinorVersion)]
[assembly: AssemblyFileVersion(BuildVersion.Version)]

// Melon Loader.
[assembly: MelonInfo(typeof(RoomFinderMod), "RoomFinder", BuildVersion.Version, "DemeoMods Team", "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("566788")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

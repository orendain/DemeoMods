﻿using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using RulesAPI.Essentials;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("RulesAPI: Essentials")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Orendain")]
[assembly: AssemblyProduct("RulesAPI: Essentials")]
[assembly: AssemblyCopyright("Copyright ©  2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8FC09405-8E06-4090-BB68-94882F0A52D8")]

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
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Melon Loader.
[assembly: MelonInfo(typeof(EssentialsMod), "RulesAPI: Essentials", "0.1.0", "Orendain", "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonID("574514")]
[assembly: VerifyLoaderVersion("0.5.3", true)]
[assembly: MelonAdditionalDependencies("RulesAPI_Core")]

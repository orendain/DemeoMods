using System.Reflection;
using System.Runtime.InteropServices;
using AdvancedStats;
using MelonLoader;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("AdvancedStats")]
[assembly: AssemblyDescription("Show text with more stats for player pieces.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("TheGrayAlien")]
[assembly: AssemblyProduct("AdvancedStats")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a1850892-02bc-41f0-9131-e63cdd61e8dc")]

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
[assembly: MelonInfo(typeof(AdvancedStatsMod), "AdvancedStats", "1.0.0", "TheGrayAlien", "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

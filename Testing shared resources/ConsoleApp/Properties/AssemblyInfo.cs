using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ConsoleApp")]
[assembly: AssemblyDescription("Console application for the post that describes various approaches to test shared resources with xUnit and Moq")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Illya Reznykov")]
[assembly: AssemblyProduct("I.Reznykov's blog http://ireznykov.com")]
[assembly: AssemblyCopyright("Copyright © 2019")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d3532f72-8d3a-479b-a27a-9cffa3cfcbab")]

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

[assembly: InternalsVisibleTo("ConsoleApp.CollectionTests")]
[assembly: InternalsVisibleTo("ConsoleApp.FailedTests")]
[assembly: InternalsVisibleTo("ConsoleApp.FakesTests")]
[assembly: InternalsVisibleTo("ConsoleApp.FixedCodeTests")]
[assembly: InternalsVisibleTo("ConsoleApp.SynchronizedTests")]


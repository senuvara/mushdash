using Sirenix.Serialization;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

[assembly: AssemblyTitle("NCalc")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NCalc")]
[assembly: AssemblyCopyright("Sebastien Ros")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("433e9af8-5d99-4c2e-a1dd-b59bf1922621")]
[assembly: AssemblyFileVersion("1.3.8.0")]
[assembly: RegisterFormatter(typeof(Vector2IntFormatter), 0)]
[assembly: RegisterFormatter(typeof(Vector3IntFormatter), 0)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[assembly: AssemblyVersion("1.3.8.0")]
[module: UnverifiableCode]

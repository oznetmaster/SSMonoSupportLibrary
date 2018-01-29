//
// System.Runtime.InteropServices.ComDefaultInterfaceAttribute
//
// Author:
//   Kazuki Oikawa  (kazuki@panicode.com)
//

using System;

#if SSHARP
namespace SSMono.Runtime.InteropServices
#else
namespace System.Runtime.InteropServices
#endif
	{
	[AttributeUsage (AttributeTargets.Class, Inherited = false)]
	[ComVisible (true)]
	public sealed class ComDefaultInterfaceAttribute : Attribute
		{
		Type _type;

		public ComDefaultInterfaceAttribute (Type defaultInterface)
			{
			_type = defaultInterface;
			}

		public Type Value { get { return _type; } }
		}
	}

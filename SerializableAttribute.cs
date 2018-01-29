using System;
using System.Runtime.InteropServices;

namespace SSMono.Runtime.Serialization
	{
	[ComVisible (true)]
	[AttributeUsageAttribute (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
	public sealed class SerializableAttribute : Attribute
		{
		public SerializableAttribute ()
			{}
		}
	}
using System;
using System.Runtime.InteropServices;

namespace SSMono.Runtime.Serialization
	{
	[AttributeUsageAttribute (AttributeTargets.Field, Inherited = false)]
	[ComVisibleAttribute (true)]
	public sealed class NonSerializedAttribute : Attribute
		{
		public NonSerializedAttribute ()
			{}
		}
	}
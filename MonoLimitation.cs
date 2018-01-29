using System;

using System.Runtime.InteropServices;

namespace SSMono
	{
	[AttributeUsage (AttributeTargets.All)]
	[Serializable]
	[ComVisible (true)]
	public class MonoLimitation : Attribute
		{
		public MonoLimitation ()
			{
			}

		public MonoLimitation (string limitation)
			{
			}
		}
	[AttributeUsage (AttributeTargets.All)]
	[Serializable]
	[ComVisible (true)]
	public class MonoTODO : Attribute
		{
		public MonoTODO ()
			{
			}

		public MonoTODO (string limitation)
			{
			}
		}
	[AttributeUsage (AttributeTargets.All)]
	[Serializable]
	[ComVisible (true)]
	public class MonoNotSupported : Attribute
		{
		public MonoNotSupported ()
			{
			}

		public MonoNotSupported (string notsupported)
			{
			}
		}
	}
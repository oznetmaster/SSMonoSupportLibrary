//
// System.TryParsers.cs
//
// Author:
//	Neil Colvin
//
// (C) 2013 Nivloc Enterprises Ltd.
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Globalization;

namespace SSMono
	{
	public static class TryParsers
		{
		public static bool ByteTryParse (string s, out byte result)
			{
			return ByteTryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool ByteTryParse (string s, NumberStyles style, IFormatProvider provider, out byte result)
			{
			if (s != null)
				{
				try
					{
					result = Byte.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(byte);
			return false;
			}

		public static bool Int16TryParse (string s, out short result)
			{
			return Int16TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool Int16TryParse (string s, NumberStyles style, IFormatProvider provider, out short result)
			{
			if (s != null)
				{
				try
					{
					result = Int16.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(short);
			return false;
			}

		public static bool Int32TryParse (string s, out int result)
			{
			return Int32TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool Int32TryParse (string s, NumberStyles style, IFormatProvider provider, out int result)
			{
			if (s != null)
				{
				try
					{
					result = Int32.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(int);
			return false;
			}

		public static bool Int64TryParse (string s, out long result)
			{
			return Int64TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool Int64TryParse (string s, NumberStyles style, IFormatProvider provider, out long result)
			{
			if (s != null)
				{
				try
					{
					result = Int64.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(long);
			return false;
			}

		public static bool UInt16TryParse (string s, out ushort result)
			{
			return UInt16TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool UInt16TryParse (string s, NumberStyles style, IFormatProvider provider, out ushort result)
			{
			if (s != null)
				{
				try
					{
					result = UInt16.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(ushort);
			return false;
			}

		public static bool UInt32TryParse (string s, out uint result)
			{
			return UInt32TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool UInt32TryParse (string s, NumberStyles style, IFormatProvider provider, out uint result)
			{
			if (s != null)
				{
				try
					{
					result = UInt32.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(uint);
			return false;
			}

		public static bool UInt64TryParse (string s, out ulong result)
			{
			return UInt64TryParse (s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool UInt64TryParse (string s, NumberStyles style, IFormatProvider provider, out ulong result)
			{
			if (s != null)
				{
				try
					{
					result = UInt64.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(ulong);
			return false;
			}

		public static bool BooleanTryParse (string s, out bool result)
			{
			if (s != null)
				{
				try
					{
					result = Boolean.Parse (s);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (bool);
			return false;
			}

		public static bool DateTimeTryParse (string s, out DateTime result)
			{
			if (s != null)
				{
				try
					{
					result = DateTime.Parse (s);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(DateTime);
			return false;
			}

		public static bool DoubleTryParse (string s, out double result)
			{
			return DoubleTryParse (s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool DoubleTryParse (string s, NumberStyles style, IFormatProvider provider, out double result)
			{
			if (s != null)
				{
				try
					{
					result = Double.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(Double);
			return false;
			}

		public static bool SingleTryParse (string s, out float result)
			{
			return SingleTryParse (s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool SingleTryParse (string s, NumberStyles style, IFormatProvider provider, out float result)
			{
			if (s != null)
				{
				try
					{
					result = Single.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default(Single);
			return false;
			}

		public static bool DecimalTryParse (string s, out decimal result)
			{
			return DecimalTryParse (s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
			}

		public static bool DecimalTryParse (string s, NumberStyles style, IFormatProvider provider, out decimal result)
			{
			if (s != null)
				{
				try
					{
					result = Decimal.Parse (s, style, provider);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (Decimal);
			return false;
			}

		public static bool DateTimeTryParse (string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (s != null)
				{
				try
					{
					result = DateTime.Parse (s, provider, styles);
					return true;
					}
				catch (Exception)
					{
					}
				}
			result = default (DateTime);
			return false;
			}

		public static bool DateTimeTryParseExact (string s, string format, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (String.IsNullOrEmpty (s) || String.IsNullOrEmpty (format))
				{
				result = default (DateTime);
				return false;
				}

			try
				{
				result = DateTime.ParseExact (s, format, provider,styles);
				return true;
				}
			catch (Exception)
				{
				}

			result = default (DateTime);
			return false;
			}

		public static bool DateTimeTryParseExact (string s, string[] formats, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
			{
			if (String.IsNullOrEmpty (s) || formats == null)
				{
				result = default (DateTime);
				return false;
				}

			foreach (var f in formats)
				{
				try
					{
					result = DateTime.ParseExact (s, f, provider, styles);
					return true;
					}
				catch (Exception)
					{
					}
				}

			result = default (DateTime);
			return false;
			}

		public static bool TimeSpanTryParse (string s, out TimeSpan result)
			{
			if (String.IsNullOrEmpty (s))
				{
				result = default (TimeSpan);
				return false;
				}

			try
				{
				result = TimeSpan.Parse (s);
				return true;
				}
			catch (Exception)
				{
				result = default (TimeSpan);
				return false;
				}
			}
		}
	}
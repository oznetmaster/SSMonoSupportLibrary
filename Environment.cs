//------------------------------------------------------------------------------
// 
// System.Environment.cs 
//
// Copyright (C) 2001 Moonlight Enterprises, All Rights Reserved
// 
// Author:         Jim Richardson, develop@wtfo-guru.com
//                 Dan Lewis (dihlewis@yahoo.co.uk)
// Created:        Saturday, August 11, 2001 
//
//------------------------------------------------------------------------------
//
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
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
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
#if SSHARP
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.CrestronDataStore;
using SSMono.Security;
using SSMono.Security.Permissions;

#endif

namespace SSMono
	{
	[ComVisible (true)]
	public static class Environment
		{
#if SSHARP
		static Environment () 
			{
			CrestronEnvironment.ProgramStatusEventHandler += new ProgramStatusEventHandler(CrestronEnvironment_ProgramStatusEventHandler);
			}

		static bool _shutdownStarted;
		static void  CrestronEnvironment_ProgramStatusEventHandler(eProgramStatusEventType programEventType)
			{
 			if (programEventType == eProgramStatusEventType.Stopping)
				_shutdownStarted = true;
			}
#endif

		/*
		 * This is the version number of the corlib-runtime interface. When
		 * making changes to this interface (by changing the layout
		 * of classes the runtime knows about, changing icall signature or
		 * semantics etc), increment this variable. Also increment the
		 * pair of this variable in the runtime in metadata/appdomain.c.
		 * Changes which are already detected at runtime, like the addition
		 * of icalls, do not require an increment.
		 */
#pragma warning disable 169
		private const int mono_corlib_version = 110;
#pragma warning restore 169

		[ComVisible (true)]
		public enum SpecialFolder
			{
#if !NETCF
			MyDocuments = 0x05,
			Desktop = 0x00,
			MyComputer = 0x11,
#endif
			Programs = 0x02,
#if !SSHARP
			Personal = 0x05,
			Favorites = 0x06,
			Startup = 0x07,
#endif
#if !NETCF
			Recent = 0x08,
			SendTo = 0x09,
#endif
#if !SSHARP
			StartMenu = 0x0b,
#endif
#if !NETCF
			MyMusic = 0x0d,
			DesktopDirectory = 0x10,
			Templates = 0x15,
#endif
			ApplicationData = 0x1a,
#if !NETCF
			LocalApplicationData = 0x1c,
			InternetCache = 0x20,
			Cookies = 0x21,
			History = 0x22,
#endif
			CommonApplicationData = 0x23,
#if !NETCF
			System = 0x25,
			ProgramFiles = 0x26,
			MyPictures = 0x27,
			CommonProgramFiles = 0x2b,
#endif
#if NET_4_0
			MyVideos = 0x0e,
#endif
#if NET_4_0
			NetworkShortcuts = 0x13,
			Fonts = 0x14,
			CommonStartMenu = 0x16,
			CommonPrograms = 0x17,
			CommonStartup = 0x18,
			CommonDesktopDirectory = 0x19,
			PrinterShortcuts = 0x1b,
			Windows = 0x24,
			UserProfile = 0x28,
			SystemX86 = 0x29,
			ProgramFilesX86 = 0x2a,
			CommonProgramFilesX86 = 0x2c,
			CommonTemplates = 0x2d,
			CommonDocuments = 0x2e,
			CommonAdminTools = 0x2f,
			AdminTools = 0x30,
			CommonMusic = 0x35,
			CommonPictures = 0x36,
			CommonVideos = 0x37,
			Resources = 0x38,
			LocalizedResources = 0x39,
			CommonOemLinks = 0x3a,
			CDBurning = 0x3b,
#endif
			}

#if NET_4_0
		public
#else
		internal
#endif
			enum SpecialFolderOption
			{
			None = 0,
			DoNotVerify = 0x4000,
			Create = 0x8000
			}

#if SSHARP
		/// <summary>
		/// Gets or sets the current directory. Actually this is supposed to get
		/// and/or set the process start directory acording to the documentation
		/// but actually test revealed at beta2 it is just Getting/Setting the CurrentDirectory
		/// </summary>
		public static string CurrentDirectory
			{
			get
				{
				if (currentDirectory != null)
					return currentDirectory;

				return InitialParametersClass.ProgramDirectory.ToString ();
				}
			set
				{
				if (value == null)
					throw new ArgumentNullException ("path");
				if (value.Trim ().Length == 0)
					throw new ArgumentException ("path string must not be an empty string or whitespace string");
				if (value.Length >= 260)
					throw new ArgumentOutOfRangeException ("path", "maximum length of path is 260 characters");
				if (value.IndexOfAny (Path.GetInvalidPathChars ()) != -1)
					throw new ArgumentException ("path", "path contains invalid characters");
				var tempPath = Path.IsPathRooted (value) ? value : Path.Combine (CurrentDirectory, value);
				if (!Directory.Exists (tempPath))
					throw new DirectoryNotFoundException ();
				currentDirectory = tempPath;
				}
			}

		private static string currentDirectory;
#else
	/// <summary>
	/// Gets the command line for this process
	/// </summary>
		public static string CommandLine
			{
			// note: security demand inherited from calling GetCommandLineArgs
			get
				{
				StringBuilder sb = new StringBuilder ();
				foreach (string str in GetCommandLineArgs ())
					{
					bool escape = false;
					string quote = "";
					string s = str;
					for (int i = 0; i < s.Length; i++)
						{
						if (quote.Length == 0 && Char.IsWhiteSpace (s[i]))
							{
							quote = "\"";
							}
						else if (s[i] == '"')
							{
							escape = true;
							}
						}
					if (escape && quote.Length != 0)
						{
						s = s.Replace ("\"", "\\\"");
						}
					sb.AppendFormat ("{0}{1}{0} ", quote, s);
					}
				if (sb.Length > 0)
					sb.Length--;
				return sb.ToString ();
				}
			}

		/// <summary>
		/// Gets or sets the current directory. Actually this is supposed to get
		/// and/or set the process start directory acording to the documentation
		/// but actually test revealed at beta2 it is just Getting/Setting the CurrentDirectory
		/// </summary>
		public static string CurrentDirectory
			{
			get
				{
				return Directory.GetCurrentDirectory ();
				}
			set
				{
				Directory.SetCurrentDirectory (value);
				}
			}

#if NET_4_5
		public static int CurrentManagedThreadId {
			get {
				return Thread.CurrentThread.ManagedThreadId;
			}
		}
#endif

		/// <summary>
		/// Gets or sets the exit code of this process
		/// </summary>
		public extern static int ExitCode
			{
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			set;
			}

		static public extern bool HasShutdownStarted
			{
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;
			}

#endif

#if SSHARP
		static public bool HasShutdownStarted
			{
			get { return _shutdownStarted; }
			}

		/// <summary>
		/// Gets the name of the local computer
		/// </summary>
		public static string MachineName
			{
			get { return machineName ?? (machineName = CrestronEthernetHelper.GetEthernetParameter (CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_HOSTNAME, 0)); }
			}

		private static string machineName;
#endif

#if !NETCF
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		extern static string GetNewLine ();

		static string nl;
#endif

		/// <summary>
		/// Gets the standard new line value
		/// </summary>
		public static string NewLine
			{
			get
				{
#if SSHARP
				return CrestronEnvironment.NewLine;
#else
				if (nl != null)
					return nl;

				nl = GetNewLine ();
				return nl;
#endif
				}
			}

		//
		// Support methods and fields for OSVersion property
		//
		private static object os;

#if SSHARP
		private static PlatformID Platform
			{
			get { return PlatformID.WinCE; }
			}
#else
		static extern PlatformID Platform
			{
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;
			}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern string GetOSVersionString ();
#endif

		/// <summary>
		/// Gets the current OS version information
		/// </summary>
		public static OperatingSystem OSVersion
			{
			get
				{
				if (os == null)
					{
#if SSHARP
					os = new OperatingSystem (PlatformID.WinCE, CrestronEnvironment.OSVersion.Version);
#else
					Version v = Version.CreateFromString (GetOSVersionString ());
					PlatformID p = Platform;
					if (p == PlatformID.MacOSX)
						p = PlatformID.Unix;
					os = new OperatingSystem (p, v);
#endif
					}
				return os as OperatingSystem;
				}
			}

		/// <summary>
		/// Get StackTrace
		/// </summary>
		public static string StackTrace
			{
#if NETCF
			get
				{
				try
					{
					throw new Exception ();
					}
				catch (Exception ex)
					{
					return ex.StackTrace;
					}
				}
#else
			[EnvironmentPermission (SecurityAction.Demand, Unrestricted = true)]
			get
				{
				System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace (0, true);
				return trace.ToString ();
				}
#endif
			}

#if !NETCF
#if !NET_2_1
		/// <summary>
		/// Get a fully qualified path to the system directory
		/// </summary>
		public static string SystemDirectory
			{
			get
				{
				return GetFolderPath (SpecialFolder.System);
				}
			}
#endif
#endif

		/// <summary>
		/// Get the number of milliseconds that have elapsed since the system was booted
		/// </summary>
#if SSHARP
		public static int TickCount
			{
			get { return CrestronEnvironment.TickCount; }
			}
#else
		public extern static int TickCount
			{
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;
			}
#endif
#if !NETCF
	/// <summary>
	/// Get UserDomainName
	/// </summary>
		public static string UserDomainName
			{
			// FIXME: this variable doesn't exist (at least not on WinXP) - reported to MS as FDBK20562
			[EnvironmentPermission (SecurityAction.Demand, Read = "USERDOMAINNAME")]
			get
				{
				return MachineName;
				}
			}

#endif

		/// <summary>
		/// Gets a flag indicating whether the process is in interactive mode
		/// </summary>
		[MonoTODO ("Currently always returns false, regardless of interactive state")]
		public static bool UserInteractive
			{
			get { return false; }
			}

#if !NETCF
	/// <summary>
	/// Get the user name of current process is running under
	/// </summary>
		public extern static string UserName
			{
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			[EnvironmentPermission (SecurityAction.Demand, Read = "USERNAME;USER")]
			get;
			}

#endif

		/// <summary>
		/// Get the version of the common language runtime 
		/// </summary>
		public static Version Version
			{
			get
				{
#if SSHARP
				return new Version ("3.5.7283.0");
#else
				return new Version (Consts.FxFileVersion);
#endif
				}
			}

#if !NETCF
	/// <summary>
	/// Get the amount of physical memory mapped to process
	/// </summary>
		[MonoTODO ("Currently always returns zero")]
		public static long WorkingSet
			{
			[EnvironmentPermission (SecurityAction.Demand, Unrestricted = true)]
			get { return 0; }
			}

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
		public extern static void Exit (int exitCode);
#endif

		/// <summary>
		/// Substitute environment variables in the argument "name"
		/// </summary>
		public static string ExpandEnvironmentVariables (string name)
			{
			if (name == null)
				throw new ArgumentNullException ("name");

			int off1 = name.IndexOf ('%');
			if (off1 == -1)
				return name;

			int len = name.Length;
			int off2 = 0;
			if (off1 == len - 1 || (off2 = name.IndexOf ('%', off1 + 1)) == -1)
				return name;

			StringBuilder result = new StringBuilder ();
			result.Append (name, 0, off1);
			Hashtable tbl = null;
			do
				{
				string var = name.Substring (off1 + 1, off2 - off1 - 1);
				string value = GetEnvironmentVariable (var);
				if (value == null && Environment.IsRunningOnWindows)
					{
					// On windows, env. vars. are case insensitive
					if (tbl == null)
						tbl = GetEnvironmentVariablesNoCase ();

					value = tbl[var] as string;
					}

				// If value not found, add %FOO to stream,
				//  and use the closing % for the next iteration.
				// If value found, expand it in place of %FOO%
				int realOldOff2 = off2;
				if (value == null)
					{
					result.Append ('%');
					result.Append (var);
					off2--;
					}
				else
					result.Append (value);
				int oldOff2 = off2;
				off1 = name.IndexOf ('%', off2 + 1);
				// If no % found for off1, don't look for one for off2
				off2 = (off1 == -1 || off2 > len - 1) ? -1 : name.IndexOf ('%', off1 + 1);
				// textLen is the length of text between the closing % of current iteration
				//  and the starting % of the next iteration if any. This text is added to output
				int textLen;
				// If no new % found, use all the remaining text
				if (off1 == -1 || off2 == -1)
					textLen = len - oldOff2 - 1;
					// If value found in current iteration, use text after current closing % and next %
				else if (value != null)
					textLen = off1 - oldOff2 - 1;
					// If value not found in current iteration, but a % was found for next iteration,
					//  use text from current closing % to the next %.
				else
					textLen = off1 - realOldOff2;
				if (off1 >= oldOff2 || off1 == -1)
					result.Append (name, oldOff2 + 1, textLen);
				}
			while (off2 > -1 && off2 < len);

			return result.ToString ();
			}

#if !NETCF
	/// <summary>
	/// Return an array of the command line arguments of the current process
	/// </summary>
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		[EnvironmentPermissionAttribute (SecurityAction.Demand, Read = "PATH")]
		public extern static string[] GetCommandLineArgs ();

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static string internalGetEnvironmentVariable (string variable);
#endif

#if SSHARP
		private static StringDictionary sdEnvironmentVariables;

		private static void CheckEnvironmentVariables ()
			{
			if (sdEnvironmentVariables != null)
				return;

			if (Interlocked.CompareExchange (ref sdEnvironmentVariables, new StringDictionary (), null) != null)
				lock (sdEnvironmentVariables.SyncRoot)
					{
					return;
					}

			lock (sdEnvironmentVariables.SyncRoot)
				{
				var global = GetGlobalVariables ().GetEnumerator ();
				while (global.MoveNext ())
					sdEnvironmentVariables[(string)global.Key] = (string)global.Value;
				var local = GetLocalVariables ().GetEnumerator ();
				while (local.MoveNext ())
					sdEnvironmentVariables[(string)local.Key] = (string)local.Value;
				}
			}

		internal static string internalGetEnvironmentVariable (string variable)
			{
			CheckEnvironmentVariables ();

			return sdEnvironmentVariables[variable];
			}

		internal static string[] GetEnvironmentVariableNames ()
			{
			CheckEnvironmentVariables ();

			lock (sdEnvironmentVariables.SyncRoot)
				{
				return sdEnvironmentVariables.Keys.Cast<string> ().ToArray ();
				}
			}
#endif

		/// <summary>
		/// Return a string containing the value of the environment
		/// variable identifed by parameter "variable"
		/// </summary>
		public static string GetEnvironmentVariable (string variable)
			{
#if !NET_2_1 && !NETCF
			if (SecurityManager.SecurityEnabled)
				{
				new EnvironmentPermission (EnvironmentPermissionAccess.Read, variable).Demand ();
				}
#endif
			return internalGetEnvironmentVariable (variable);
			}

		private static Hashtable GetEnvironmentVariablesNoCase ()
			{
			var vars = new Hashtable (CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);

			foreach (string name in GetEnvironmentVariableNames ())
				vars[name] = internalGetEnvironmentVariable (name);

			return vars;
			}

		/// <summary>
		/// Return a set of all environment variables and their values
		/// </summary>
#if !NET_2_1 && !NETCF
		public static IDictionary GetEnvironmentVariables ()
			{
			StringBuilder sb = null;
			if (SecurityManager.SecurityEnabled)
				{
				// we must have access to each variable to get the lot
				sb = new StringBuilder ();
				// but (performance-wise) we do not want a stack-walk
				// for each of them so we concatenate them
				}

			Hashtable vars = new Hashtable ();
			foreach (string name in GetEnvironmentVariableNames ())
				{
				vars[name] = internalGetEnvironmentVariable (name);
				if (sb != null)
					{
					sb.Append (name);
					sb.Append (";");
					}
				}

			if (sb != null)
				{
				new EnvironmentPermission (EnvironmentPermissionAccess.Read, sb.ToString ()).Demand ();
				}
			return vars;
			}
		[EnvironmentPermission (SecurityAction.Demand, Unrestricted=true)]
#else
		public static IDictionary GetEnvironmentVariables ()
			{
			Hashtable vars = new Hashtable ();
			foreach (string name in GetEnvironmentVariableNames ())
				vars[name] = internalGetEnvironmentVariable (name);
			return vars;
			}
#endif

#if !NETCF
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern static string GetWindowsFolderPath (int folder);

#endif

		/// <summary>
		/// Returns the fully qualified path of the
		/// folder specified by the "folder" parameter
		/// </summary>
		public static string GetFolderPath (SpecialFolder folder)
			{
			return GetFolderPath (folder, SpecialFolderOption.None);
			}

#if NET_4_0
		public
#endif

		private static string GetFolderPath (SpecialFolder folder, SpecialFolderOption option)
			{
			string dir = null;
#if SSHARP
			switch (folder)
				{
				case SpecialFolder.ApplicationData:
					dir = String.Format ("\\Nvram\\App{0:D2}\\", InitialParametersClass.ApplicationNumber);
					if (option == SpecialFolderOption.Create)
						Directory.Create (dir);
					break;
				case SpecialFolder.Programs:
					dir = InitialParametersClass.ProgramDirectory.ToString ();
					break;
				case SpecialFolder.CommonApplicationData:
					dir = "\\Nvram\\";
					break;
				}

#else
			SecurityManager.EnsureElevatedPermissions (); // this is a no-op outside moonlight


			if (Environment.IsRunningOnWindows)
				dir = GetWindowsFolderPath ((int)folder);
			else
				dir = UnixGetFolderPath (folder, option);

#if !NET_2_1
			if ((dir != null) && (dir.Length > 0) && SecurityManager.SecurityEnabled)
				{
				new FileIOPermission (FileIOPermissionAccess.PathDiscovery, dir).Demand ();
				}
#endif
#endif
			return dir;
			}

#if !NETCF
		private static string ReadXdgUserDir (string config_dir, string home_dir, string key, string fallback)
			{
			string env_path = internalGetEnvironmentVariable (key);
			if (env_path != null && env_path != String.Empty)
				{
				return env_path;
				}

			string user_dirs_path = Path.Combine (config_dir, "user-dirs.dirs");

			if (!File.Exists (user_dirs_path))
				{
				return Path.Combine (home_dir, fallback);
				}

			try
				{
				using (StreamReader reader = new StreamReader (user_dirs_path))
					{
					string line;
					while ((line = reader.ReadLine ()) != null)
						{
						line = line.Trim ();
						int delim_index = line.IndexOf ('=');
						if (delim_index > 8 && line.Substring (0, delim_index) == key)
							{
							string path = line.Substring (delim_index + 1).Trim ('"');
							bool relative = false;

							if (path.StartsWithOrdinalUnchecked ("$HOME/"))
								{
								relative = true;
								path = path.Substring (6);
								}
							else if (!path.StartsWithOrdinalUnchecked ("/"))
								{
								relative = true;
								}

							return relative ? Path.Combine (home_dir, path) : path;
							}
						}
					}
				}
			catch (FileNotFoundException)
				{
				}

			return Path.Combine (home_dir, fallback);
			}


		// the security runtime (and maybe other parts of corlib) needs the
		// information to initialize themselves before permissions can be checked
		internal static string UnixGetFolderPath (SpecialFolder folder, SpecialFolderOption option)
			{
			string home = internalGetHome ();

			// http://freedesktop.org/Standards/basedir-spec/basedir-spec-0.6.html

			// note: skip security check for environment variables
			string data = internalGetEnvironmentVariable ("XDG_DATA_HOME");
			if ((data == null) || (data == String.Empty))
				{
				data = Path.Combine (home, ".local");
				data = Path.Combine (data, "share");
				}

			// note: skip security check for environment variables
			string config = internalGetEnvironmentVariable ("XDG_CONFIG_HOME");
			if ((config == null) || (config == String.Empty))
				{
				config = Path.Combine (home, ".config");
				}

			switch (folder)
				{
				// MyComputer is a virtual directory
				case SpecialFolder.MyComputer:
					return String.Empty;

				// personal == ~
				case SpecialFolder.Personal:
#if MONOTOUCH
				return Path.Combine (home, "Documents");
#else
					return home;
#endif
				// use FDO's CONFIG_HOME. This data will be synced across a network like the windows counterpart.
				case SpecialFolder.ApplicationData:
#if MONOTOUCH
			{
				string dir = Path.Combine (Path.Combine (home, "Documents"), ".config");
				if (option == SpecialFolderOption.Create){
					if (!Directory.Exists (dir))
						Directory.CreateDirectory (dir);
				}
				return dir;
			}
#else
					return config;
#endif
				//use FDO's DATA_HOME. This is *NOT* synced
				case SpecialFolder.LocalApplicationData:
#if MONOTOUCH
			{
				string dir = Path.Combine (home, "Documents");
				if (!Directory.Exists (dir))
					Directory.CreateDirectory (dir);

				return dir;
			}
#else
					return data;
#endif

				case SpecialFolder.Desktop:
				case SpecialFolder.DesktopDirectory:
					return ReadXdgUserDir (config, home, "XDG_DESKTOP_DIR", "Desktop");

				case SpecialFolder.MyMusic:
					if (Platform == PlatformID.MacOSX)
						return Path.Combine (home, "Music");
					else
						return ReadXdgUserDir (config, home, "XDG_MUSIC_DIR", "Music");

				case SpecialFolder.MyPictures:
					if (Platform == PlatformID.MacOSX)
						return Path.Combine (home, "Pictures");
					else
						return ReadXdgUserDir (config, home, "XDG_PICTURES_DIR", "Pictures");

				case SpecialFolder.Templates:
					return ReadXdgUserDir (config, home, "XDG_TEMPLATES_DIR", "Templates");
#if NET_4_0
			case SpecialFolder.MyVideos:
				return ReadXdgUserDir (config, home, "XDG_VIDEOS_DIR", "Videos");
#endif
#if NET_4_0
			case SpecialFolder.CommonTemplates:
				return "/usr/share/templates";
			case SpecialFolder.Fonts:
				if (Platform == PlatformID.MacOSX)
					return Path.Combine (home, "Library", "Fonts");
				
				return Path.Combine (home, ".fonts");
#endif
				// these simply dont exist on Linux
				// The spec says if a folder doesnt exist, we
				// should return ""
				case SpecialFolder.Favorites:
					if (Platform == PlatformID.MacOSX)
						return Path.Combine (home, "Library", "Favorites");
					else
						return String.Empty;

				case SpecialFolder.ProgramFiles:
					if (Platform == PlatformID.MacOSX)
						return "/Applications";
					else
						return String.Empty;

				case SpecialFolder.InternetCache:
					if (Platform == PlatformID.MacOSX)
						return Path.Combine (home, "Library", "Caches");
					else
						return String.Empty;

#if NET_4_0
				// #2873
			case SpecialFolder.UserProfile:
				return home;
#endif

				case SpecialFolder.Programs:
				case SpecialFolder.SendTo:
				case SpecialFolder.StartMenu:
				case SpecialFolder.Startup:
				case SpecialFolder.Cookies:
				case SpecialFolder.History:
				case SpecialFolder.Recent:
				case SpecialFolder.CommonProgramFiles:
				case SpecialFolder.System:
#if NET_4_0
			case SpecialFolder.NetworkShortcuts:
			case SpecialFolder.CommonStartMenu:
			case SpecialFolder.CommonPrograms:
			case SpecialFolder.CommonStartup:
			case SpecialFolder.CommonDesktopDirectory:
			case SpecialFolder.PrinterShortcuts:
			case SpecialFolder.Windows:
			case SpecialFolder.SystemX86:
			case SpecialFolder.ProgramFilesX86:
			case SpecialFolder.CommonProgramFilesX86:
			case SpecialFolder.CommonDocuments:
			case SpecialFolder.CommonAdminTools:
			case SpecialFolder.AdminTools:
			case SpecialFolder.CommonMusic:
			case SpecialFolder.CommonPictures:
			case SpecialFolder.CommonVideos:
			case SpecialFolder.Resources:
			case SpecialFolder.LocalizedResources:
			case SpecialFolder.CommonOemLinks:
			case SpecialFolder.CDBurning:
#endif
					return String.Empty;
				// This is where data common to all users goes
				case SpecialFolder.CommonApplicationData:
					return "/usr/share";
				default:
					throw new ArgumentException ("Invalid SpecialFolder");
				}
			}


		[EnvironmentPermission (SecurityAction.Demand, Unrestricted = true)]
		public static string[] GetLogicalDrives ()
			{
			return GetLogicalDrivesInternal ();
			}

#if !NET_2_1
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private static extern void internalBroadcastSettingChange ();
#endif
#endif

#if !NET_2_1
		public static string GetEnvironmentVariable (string variable, EnvironmentVariableTarget target)
			{
			switch (target)
				{
				case EnvironmentVariableTarget.Process:
					return GetEnvironmentVariable (variable);
				case EnvironmentVariableTarget.Machine:
#if !NETCF
					new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
#endif
					if (!IsRunningOnWindows)
						return null;
#if SSHARP
					{
					string value;
					var result = CrestronDataStoreStatic.GetGlobalStringValue (variable, out value);
					return result == CrestronDataStore.CDS_ERROR.CDS_SUCCESS ? value : null;
					}
#else
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment"))
						{
						object regvalue = env.GetValue (variable);
						return (regvalue == null) ? null : regvalue.ToString ();
						}
#endif
				case EnvironmentVariableTarget.User:
#if !NETCF
					new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
#endif
					if (!IsRunningOnWindows)
						return null;
#if SSHARP
					{
					string value;
					var result = CrestronDataStoreStatic.GetLocalStringValue (variable, out value);
					return result == CrestronDataStore.CDS_ERROR.CDS_SUCCESS ? value : null;
					}
#else
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment", false))
						{
						object regvalue = env.GetValue (variable);
						return (regvalue == null) ? null : regvalue.ToString ();
						}
#endif
				default:
					throw new ArgumentException ("target");
				}
			}

#if SSHARP
		private static bool dataStoreInitialized = false;

		private static void InitializeDataStore ()
			{
			if (!dataStoreInitialized)
				{
				CrestronDataStoreStatic.InitCrestronDataStore ();
				dataStoreInitialized = true;
				}
			}

		private static IDictionary GetGlobalVariables ()
			{
			InitializeDataStore ();

			var dict = new Hashtable ();
			string name = String.Empty;
			CrestronDataStore.recInfo info;
			CrestronDataStore.CDS_ERROR result;
			while ((result = CrestronDataStoreStatic.GetNextGlobalTag (ref name, out info, CrestronDataStore.CDS_ACTION.CDS_NAMEONLY))
			       == CrestronDataStore.CDS_ERROR.CDS_SUCCESS || result == CrestronDataStore.CDS_ERROR.CDS_END_OF_TABLE)
				{
				if (info.type == CrestronDataStore.CDS_TYPE.String)
					{
					string value;
					CrestronDataStoreStatic.GetGlobalStringValue (name, out value);
					dict.Add (name, value);
					}

				if (result == CrestronDataStore.CDS_ERROR.CDS_END_OF_TABLE)
					break;
				}
			return dict;
			}

		private static IDictionary GetLocalVariables ()
			{
			InitializeDataStore ();

			var dict = new Hashtable ();
			string name = String.Empty;
			CrestronDataStore.recInfo info;
			CrestronDataStore.CDS_ERROR result;
			while ((result = CrestronDataStoreStatic.GetNextLocalTag (ref name, out info)) == CrestronDataStore.CDS_ERROR.CDS_SUCCESS || result == CrestronDataStore.CDS_ERROR.CDS_END_OF_TABLE)
				{
				if (info.type == CrestronDataStore.CDS_TYPE.String)
					{
					string value;
					CrestronDataStoreStatic.GetLocalStringValue (name, out value);
					dict.Add (name, value);
					}

				if (result == CrestronDataStore.CDS_ERROR.CDS_END_OF_TABLE)
					break;
				}
			return dict;
			}
#endif

		public static IDictionary GetEnvironmentVariables (EnvironmentVariableTarget target)
			{
			IDictionary variables = (IDictionary)new Hashtable ();
			switch (target)
				{
				case EnvironmentVariableTarget.Process:
					variables = GetEnvironmentVariables ();
					break;
				case EnvironmentVariableTarget.Machine:
#if !NETCF
					new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
#endif
					if (IsRunningOnWindows)
						{
#if SSHARP
						variables = GetGlobalVariables ();
#else
						using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment"))
							{
							string[] value_names = env.GetValueNames ();
							foreach (string value_name in value_names)
								variables.Add (value_name, env.GetValue (value_name));
							}
#endif
						}
					break;
				case EnvironmentVariableTarget.User:
#if !NETCF
					new EnvironmentPermission (PermissionState.Unrestricted).Demand ();
#endif
					if (IsRunningOnWindows)
						{
#if SSHARP
						variables = GetLocalVariables ();
#else
						using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment"))
							{
							string[] value_names = env.GetValueNames ();
							foreach (string value_name in value_names)
								variables.Add (value_name, env.GetValue (value_name));
							}
#endif
						}
					break;
				default:
					throw new ArgumentException ("target");
				}
			return variables;
			}

#if !NETCF
		[EnvironmentPermission (SecurityAction.Demand, Unrestricted = true)]
#endif

		public static void SetEnvironmentVariable (string variable, string value)
			{
			SetEnvironmentVariable (variable, value, EnvironmentVariableTarget.Process);
			}

#if !NETCF
		[EnvironmentPermission (SecurityAction.Demand, Unrestricted = true)]
#endif

		public static void SetEnvironmentVariable (string variable, string value, EnvironmentVariableTarget target)
			{
			if (variable == null)
				throw new ArgumentNullException ("variable");
			if (variable == String.Empty)
				throw new ArgumentException ("String cannot be of zero length.", "variable");
			if (variable.IndexOf ('=') != -1)
				throw new ArgumentException ("Environment variable name cannot contain an equal character.", "variable");
			if (variable[0] == '\0')
				throw new ArgumentException ("The first char in the string is the null character.", "variable");

			switch (target)
				{
				case EnvironmentVariableTarget.Process:
					InternalSetEnvironmentVariable (variable, value);
					break;
				case EnvironmentVariableTarget.Machine:
					if (!IsRunningOnWindows)
						return;
#if SSHARP
					if (String.IsNullOrEmpty (value))
						CrestronDataStoreStatic.clearGlobal (value);
					else
						CrestronDataStoreStatic.SetGlobalStringValue (variable, value);
#else
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true))
						{
						if (String.IsNullOrEmpty (value))
							env.DeleteValue (variable, false);
						else
							env.SetValue (variable, value);
#if !NETCF
						internalBroadcastSettingChange ();
#endif
						}
#endif
					break;
				case EnvironmentVariableTarget.User:
					if (!IsRunningOnWindows)
						return;
#if SSHARP
					if (String.IsNullOrEmpty (value))
						CrestronDataStoreStatic.clearLocal (value);
					else
						CrestronDataStoreStatic.SetLocalStringValue (variable, value);
#else
					using (Microsoft.Win32.RegistryKey env = Microsoft.Win32.Registry.CurrentUser.OpenSubKey ("Environment", true))
						{
						if (String.IsNullOrEmpty (value))
							env.DeleteValue (variable, false);
						else
							env.SetValue (variable, value);
#if !NETCF
						internalBroadcastSettingChange ();
#endif
						}
#endif
					break;
				default:
					throw new ArgumentException ("target");
				}
			}

#if SSHARP
		internal static void InternalSetEnvironmentVariable (string variable, string value)
			{
			lock (sdEnvironmentVariables.SyncRoot)
				{
				if (String.IsNullOrEmpty (value))
					sdEnvironmentVariables.Remove (variable);
				else
					sdEnvironmentVariables[variable] = value;
				}
			}
#endif

#if !NETCF
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal static extern void InternalSetEnvironmentVariable (string variable, string value);
#endif
#endif

#if NET_4_0 || SSHARP
		[SecurityPermission (SecurityAction.LinkDemand, UnmanagedCode = true)]
		public static void FailFast (string message)
			{
#if SSHARP
			ErrorLog.Error (String.Format ("FailFast: {0}\r\n{1}", message, StackTrace));

			if (Debugger.IsAttached)
				{
				Debugger.WriteLine (message);
				Debugger.Break ();
				}

			string response = String.Empty;
			CrestronConsole.SendControlSystemCommand ("stopprog -p:" + InitialParametersClass.ApplicationNumber, ref response);
#else
			throw new NotImplementedException ();
#endif
			}

		[SecurityCritical]
		public static void FailFast (string message, Exception exception)
			{
#if SSHARP
			if (exception == null)
				{
				FailFast (message);
				return;
				}

			ErrorLog.Exception ("FailFast: " + message, exception);

			if (Debugger.IsAttached)
				{
				Debugger.WriteLine (String.Format ("{0} : {1}", message, exception.Message));
				Debugger.Break ();
				}

			string response = String.Empty;
			CrestronConsole.SendControlSystemCommand ("stopprog -p:" + InitialParametersClass.ApplicationNumber, ref response);
#else
			throw new NotImplementedException ();
#endif
			}
#endif

#if !NETCF
#if NET_4_0
		public static bool Is64BitOperatingSystem {
			get { return IntPtr.Size == 8; } // FIXME: is this good enough?
		}

		public static int SystemPageSize {
			get { return GetPageSize (); }
		}
#endif

#if NET_4_0
		public
#else
		internal
#endif
 static bool Is64BitProcess
			{
			get { return IntPtr.Size == 8; }
			}

		public static extern int ProcessorCount
			{
			[EnvironmentPermission (SecurityAction.Demand, Read = "NUMBER_OF_PROCESSORS")]
			[MethodImplAttribute (MethodImplOptions.InternalCall)]
			get;
			}
#endif
		// private methods
#if MOBILE 
		internal const bool IsRunningOnWindows = false;
#else
#if SSHARP
		public
#else
		internal
#endif
			static bool IsRunningOnWindows
			{
			get { return ((int)Platform < 4); }
			}
#endif

#if !NETCF
#if !NET_2_1
		//
		// Used by gacutil.exe
		//
#pragma warning disable 169
		private static string GacPath
			{
			get
				{
				if (Environment.IsRunningOnWindows)
					{
					/* On windows, we don't know the path where mscorlib.dll will be installed */
					string corlibDir = new DirectoryInfo (Path.GetDirectoryName (typeof (int).Assembly.Location)).Parent.Parent.FullName;
					return Path.Combine (Path.Combine (corlibDir, "mono"), "gac");
					}

				return Path.Combine (Path.Combine (internalGetGacPath (), "mono"), "gac");
				}
			}
#pragma warning restore 169
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static string internalGetGacPath ();
#endif
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern static string[] GetLogicalDrivesInternal ();

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		private extern static string[] GetEnvironmentVariableNames ();

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static string GetMachineConfigPath ();

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static string internalGetHome ();

		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern static int GetPageSize ();

#endif
#if SSHARP
		public
#else
		internal
#endif
			static bool IsUnix
			{
			get
				{
				int platform = (int)Platform;

				return (platform == 4 || platform == 128 || platform == 6);
				}
			}

#if SSHARP
		public
#else
		internal
#endif
			static bool IsMacOS
			{
			get
				{
#if SSHARP
				return false;
#else
				return Environment.Platform == PlatformID.MacOSX;
#endif
				}
			}
		}
	}
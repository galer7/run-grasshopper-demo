using System.Reflection;
using System.Runtime.InteropServices;

// Code from https://github.com/mcneel/compute.rhino3d/blob/8.x/src/compute.geometry/Resolver.cs
namespace RhinoInside
{
    public class Resolver
    {
        /// <summary>
        /// Set up an assembly resolver to load RhinoCommon and other Rhino
        /// assemblies from where Rhino is installed
        /// </summary>
        public static void Initialize()
        {
            Console.WriteLine("Initializing RhinoInside.Resolver");
            if (System.IntPtr.Size != 8)
                throw new Exception("Only 64 bit applications can use RhinoInside");
            AppDomain.CurrentDomain.AssemblyResolve += ResolveForRhinoAssemblies;
        }

        static string _rhinoSystemDirectory;

        /// <summary>
        /// Directory used by assembly resolver to attempt load core Rhino assemblies. If not manually set,
        /// this will be determined by inspecting the registry
        /// 
        /// This is the C:/Program Files/Rhino 8/System directory on Windows
        /// This is the Rhinoceros.app/Contents/Frameworks directory on Mac
        /// </summary>
        public static string RhinoSystemDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_rhinoSystemDirectory))
                    _rhinoSystemDirectory = FindRhinoSystemDirectory();
                return _rhinoSystemDirectory;
            }
            set
            {
                _rhinoSystemDirectory = value;
            }
        }

        /// <summary>
        /// Whether or not to use the newest installation of Rhino on the system. By default the resolver will only use an
        /// installation with a matching major version.
        /// </summary>
        public static bool UseLatest { get; set; } = true;

        public static string AssemblyPathFromName(string systemDirectory, string name)
        {
            if (name == null || name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                return null;

            // load Microsoft.macOS in the default context as xamarin initialization requires it there
            if (name == "Microsoft.macOS")
                return null;

            string path = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = Path.Combine(systemDirectory, "RhCore.framework/Resources", name + ".dll");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = Path.Combine(systemDirectory, "netcore", name + ".dll");
                if (!File.Exists(path))
                    path = Path.Combine(systemDirectory, name + ".dll");
                //if (!File.Exists(path))
                //{
                //    var intPath = typeof(int).Assembly.Location;
                //    string directory = System.IO.Path.GetDirectoryName(intPath);
                //    path = Path.Combine(directory, name + ".dll");
                //    if (!File.Exists(path) || name.Contains(".Drawing") || name.Contains("WindowsBase"))
                //    {
                //        int index = directory.IndexOf("NETCORE", StringComparison.OrdinalIgnoreCase);
                //        directory = directory.Substring(0, index) + "WindowsDesktop" + directory.Substring(index + "NETCORE".Length);
                //        path = Path.Combine(directory, name + ".dll");
                //    }
                //}
            }
            return path;
        }

        static Assembly ResolveForRhinoAssemblies(object sender, ResolveEventArgs args)
        {
            string path = AssemblyPathFromName(RhinoSystemDirectory, args.Name);
            if (File.Exists(path))
                return Assembly.LoadFrom(path);
            return null;
        }

        static string FindRhinoSystemDirectory()
        {
            var major = Assembly.GetExecutingAssembly().GetName().Version.Major;

            if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                string baseName = @"SOFTWARE\McNeel\Rhinoceros";
                using var baseKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(baseName);
                string[] children = baseKey.GetSubKeyNames();
                Array.Sort(children);
                string versionName = "";
                for (int i = children.Length - 1; i >= 0; i--)
                {
                    // 20 Jan 2020 S. Baer (https://github.com/mcneel/rhino.inside/issues/248)
                    // A generic double.TryParse is failing when run under certain locales.
                    if (double.TryParse(children[i], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double d))
                    {
                        if (d < 8.0)
                            continue;

                        versionName = children[i];

                        if (!UseLatest && (int)Math.Floor(d) != major)
                            continue;

                        using var installKey = baseKey.OpenSubKey($"{versionName}\\Install");
                        string corePath = installKey.GetValue("CoreDllPath") as string;
                        if (System.IO.File.Exists(corePath))
                        {
                            return System.IO.Path.GetDirectoryName(corePath);
                        }
                    }
                }
            }

            return null;
        }
    }
}
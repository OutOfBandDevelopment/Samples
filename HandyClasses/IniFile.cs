using System.Runtime.InteropServices;
using System.Text;

namespace OobDev.Tools
{
    public class IniFile
    {

        public IniFile(string iniPath)
        {
            this.Path = System.IO.Path.GetFullPath(iniPath);
            this.ReadLength = 255;
            this.DefaultSection = "Global";
        }
        public IniFile(string iniPath, string defaultSection)
            : this(iniPath)
        {
            this.DefaultSection = defaultSection;
        }

        public string this[string key]
        {
            get { return this[this.DefaultSection, key]; }
            set { this[this.DefaultSection, key] = value; }
        }
        public string this[string section, string key]
        {
            get { return this[section, key, string.Empty]; }
            set { this[section, key] = value; }
        }
        public string this[string section, string key, string @default]
        {
            get { return this.Read(section, key, @default); }
            set { this.Write(section, key, value); }
        }

        public string Path { get; private set; }
        public int ReadLength { get; set; }
        public string DefaultSection { get; private set; }

        [DllImport("kernel32")]
        private static extern uint WritePrivateProfileString(
            string section,
            string key,
            string val,
            string path);

        [DllImport("kernel32")]
        private static extern uint GetPrivateProfileString(
            string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string path);

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.Path);
        }

        public string Read(string section, string key)
        {
            return this.Read(section, key, string.Empty);
        }
        public string Read(string section, string key, string defaultValue)
        {
            var sb = new StringBuilder(this.ReadLength);
            if (defaultValue == null) defaultValue = string.Empty;
            var ret = GetPrivateProfileString(section,
                                              key,
                                              defaultValue,
                                              sb,
                                              sb.Capacity,
                                              this.Path);
            return sb.ToString();
        }
    }
}

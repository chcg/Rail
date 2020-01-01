using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rail.Mvvm
{
    [AttributeUsage(AttributeTargets.Assembly)]
    [ComVisible(true)]
    public sealed class AssemblyProjectVersionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the Mvvm.Model.AssemblyProjectVersionAttribute class, specifying the project file version.
        /// </summary>
        /// <param name="version">The project file version.</param>
        /// <exception cref="System.ArgumentNullException">version is null.</exception>
        public AssemblyProjectVersionAttribute(string version)
        {
            this.Version = version;
        }
                
        /// <summary>
        /// Gets the project file version resource name.
        /// </summary>
        /// <remarks>A string containing the project file version resource name.</remarks>
        public string Version { get; private set; }
    }
}

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using VSPremake.Resources;

namespace VSPremake
{

    /// <summary>
    /// User interface for the args option
    /// </summary>
    public class OptionPage : DialogPage
    {
        //User commandline arguments
        private string args = "vs2022";

        [Category("VSPremake")]
        [DisplayName("Arguments")]
        [Description("Provide arguments for premake here.")]
        public string Arguments
        {
            get { return args; }
            set { args = value; }
        }

        //Location and file name of user provided
        private string execLocation = "";

        [Category("VSPremake")]
        [DisplayName("Executable Location")]
        [Description("Use a different executable.")]
        public string ExecutableLocation
        {
            get { return execLocation; }
            set { execLocation = value; }
        }

        private Resources.ExecutableLocationPicker _picker;

        [BrowsableAttribute(false)]
        protected override IWin32Window Window
        {
            get
            {
                if (_picker == null)
                {
                    _picker = new Resources.ExecutableLocationPicker();
                    // Optionally bind properties here
                }
                return _picker;
            }
        }

    }
}

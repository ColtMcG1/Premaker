using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Premaker.Resources
{
    public partial class ExecutableLocationPicker : UserControl
    {
        public ExecutableLocationPicker()
        {
            InitializeComponent();
        }

        private void ExecutableLocationPicker_Load(object sender, EventArgs e)
        {

        }
        private void pickerOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.CheckFileExists)
            {
                fileLocation.Text = openFileDialog1.FileName;
            }
        }
    }
}

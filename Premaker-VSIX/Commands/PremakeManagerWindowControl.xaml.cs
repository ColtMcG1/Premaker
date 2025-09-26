using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Premaker
{
    /// <summary>
    /// Interaction logic for PremakeManagerWindowControl.
    /// </summary>
    public partial class PremakeManagerWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PremakeManagerWindowControl"/> class.
        /// </summary>
        public PremakeManagerWindowControl()
        {
            this.InitializeComponent();
            OutputRichTextBox.Document.Blocks.Clear();
            OutputRichTextBox.Focus();
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void OutputRichTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
        
    }
}
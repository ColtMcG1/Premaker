

using System.Windows;
using System.Windows.Controls;


namespace Premaker
{
    public class InputDialog : Window
    {
        private TextBox textBox;
        public string InputText { get; private set; }

        public InputDialog(string prompt, string title, string defaultValue = "")
        {
            this.Title = title;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.ResizeMode = ResizeMode.NoResize;
            this.SizeToContent = SizeToContent.WidthAndHeight;

            StackPanel stack = new StackPanel { Margin = new Thickness(16) };

            TextBlock label = new TextBlock { Text = prompt, Margin = new Thickness(0, 0, 0, 8), TextWrapping = TextWrapping.Wrap };
            stack.Children.Add(label);

            this.textBox = new TextBox { Text = defaultValue ?? string.Empty, MinWidth = 250, Height = 24 };
            stack.Children.Add(this.textBox);

            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };

            Button ok = new Button { Content = "OK", Width = 75, IsDefault = true, Margin = new Thickness(0, 0, 8, 0) };
            ok.Click += (s, e) => { this.InputText = this.textBox.Text; this.DialogResult = true; };

            Button cancel = new Button { Content = "Cancel", Width = 75, IsCancel = true };
            cancel.Click += (s, e) => { this.DialogResult = false; };

            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);

            stack.Children.Add(buttons);

            this.Content = stack;
        }
    }
}



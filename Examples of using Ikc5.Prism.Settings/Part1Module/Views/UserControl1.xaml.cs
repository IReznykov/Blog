using System.Windows.Controls;
using Part1Module.Models;

namespace Part1Module.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(ISettings settingsModel)
		{
			DataContext = settingsModel;
			InitializeComponent();
        }
    }
}

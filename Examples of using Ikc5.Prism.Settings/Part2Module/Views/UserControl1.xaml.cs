using System.Windows.Controls;
using Part2Module.Models;

namespace Part2Module.Views
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

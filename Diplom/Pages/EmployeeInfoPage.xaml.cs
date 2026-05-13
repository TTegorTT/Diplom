using System.Windows;
using System.Windows.Controls;
using Diplom.Classes;

namespace Diplom.Pages
{
    public partial class EmployeeInfoPage : Page
    {
        public EmployeeViewModel Employee { get; set; }

        public EmployeeInfoPage(EmployeeViewModel employee)
        {
            InitializeComponent();
            Employee = employee;
            DataContext = this; 
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }

}
using LibraryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для PublisherPage.xaml
    /// </summary>
    public partial class PublisherPage : Page
    {
        public PublisherPage()
        {
            InitializeComponent();
            DataContext = new PublisherViewModel();
        }
    }
}

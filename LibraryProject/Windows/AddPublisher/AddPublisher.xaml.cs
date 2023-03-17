using LibraryProject.DB;
using LibraryProject.ViewModel.AddViewModel;
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
using System.Windows.Shapes;

namespace LibraryProject.Windows.AddPublisher
{
    /// <summary>
    /// Логика взаимодействия для AddPublisher.xaml
    /// </summary>
    public partial class AddPublisher : Window
    {
        public AddPublisher()
        {
            InitializeComponent();
            DataContext = new AddPublisherViewModel(null);
        }
        public AddPublisher(Publisher publisher)
        {
            InitializeComponent();
            DataContext = new AddPublisherViewModel(publisher);
        }
    }
}

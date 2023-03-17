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

namespace LibraryProject.Windows.AddShelf
{
    /// <summary>
    /// Логика взаимодействия для AddShelf.xaml
    /// </summary>
    public partial class AddShelf : Window
    {
        public AddShelf()
        {
            InitializeComponent();
            DataContext = new AddShelfViewModel(null);
        }
        public AddShelf(Shelf shelf)
        {
            InitializeComponent();
            DataContext = new AddShelfViewModel(shelf);
        }

    }
}

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

namespace LibraryProject.Windows.AddGenre
{
    /// <summary>
    /// Логика взаимодействия для AddGenre.xaml
    /// </summary>
    public partial class AddGenre : Window
    {
        public AddGenre()
        {
            InitializeComponent();
            DataContext = new AddGenreViewModel(null);
        }
        public AddGenre(Genre genre)
        {
            InitializeComponent();
            DataContext = new AddGenreViewModel(genre);
        }
    }
}

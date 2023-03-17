using LibraryProject.DB;
using LibraryProject.ViewModel.AddViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LibraryProject.Windows.AddBook
{
    /// <summary>
    /// Логика взаимодействия для AddAuthorBook.xaml
    /// </summary>
    public partial class AddAuthorBook : Window
    {
        public AddAuthorBook()
        {
            InitializeComponent();
            DataContext = new AddAuthorBookViewModel(null, null);
        }

        public AddAuthorBook(Book book, ObservableCollection<Author> SelectedBookAuthors)
        {
            InitializeComponent();
            DataContext = new AddAuthorBookViewModel(book, SelectedBookAuthors);
        }
    }
}

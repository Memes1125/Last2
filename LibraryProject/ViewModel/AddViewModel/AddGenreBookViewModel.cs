using LibraryProject.Core;
using LibraryProject.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel.AddViewModel
{
    public class AddGenreBookViewModel: BaseViewModel
    {
        public Library_DBContext dbContext = new Library_DBContext();
        public List<Genre> Genres { get; set; }

        private Genre selectedGenre { get; set; }
        public Genre SelectedGenre
        {
            get => selectedGenre;
            set
            {
                selectedGenre = value;
                SignalChanged();
            }
        }

        public List<Genre> searchResult { get; set; }

        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }
        public CustomCommand Cancel { get; set; }
        public CustomCommand Save { get; set; }
        public AddGenreBookViewModel(Book book, ObservableCollection<Genre> SelectedBookGenres)
        {
            Genres = dbContext.Genres.ToList();

            Save = new CustomCommand(() =>
            {
                if(book != null)
                {
                    BookGenreCross bookGenreCross = new BookGenreCross();
                    bookGenreCross.GenreId = SelectedGenre.Id;
                    bookGenreCross.BookId = book.Id;
                    dbContext.BookGenreCrosses.Add(bookGenreCross);
                    dbContext.SaveChanges();
                }
                else
                {
                    SelectedBookGenres.Add(SelectedGenre);
                }

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this) CloseModalWindow(window);
                }

            });


            Cancel = new CustomCommand(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this) CloseModalWindow(window);
                }

            });
            searchResult = dbContext.Genres.ToList();
        }

        private void CloseModalWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        public void Search()
        {
            var search = SearchText.ToLower();
            searchResult = dbContext.Genres.Where(s => s.GenreName.ToLower().Contains(search)).ToList();
            Genres = searchResult;
            SignalChanged("Genres");
        }
    }
}

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
    public class AddAuthorBookViewModel: BaseViewModel
    {
        #region Properties
        
        public Library_DBContext dbContext = new Library_DBContext();
        public List<Author> Authors { get; set; }
        private List<Author> searchResult;

        private Author selectedAuthor { get; set; }
        public Author SelectedAuthor
        {
            get => selectedAuthor;
            set
            {
                selectedAuthor = value;
                SignalChanged();
            }
        }

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
        #endregion

        public CustomCommand Save { get; set; }
        public CustomCommand Cancel { get; set; }
        public AddAuthorBookViewModel(Book book, ObservableCollection<Author> SelectedBookAuthors)
        {
            Authors = dbContext.Authors.ToList();

            Save = new CustomCommand(() =>
            {
                if(book != null)
                {
                    BookAuthorCross bookAuthorCross = new BookAuthorCross();
                    bookAuthorCross.AuthorId = SelectedAuthor.Id;
                    bookAuthorCross.BookId = book.Id;
                    dbContext.BookAuthorCrosses.Add(bookAuthorCross);
                    dbContext.SaveChanges();
                }
                else
                {
                    SelectedBookAuthors.Add(SelectedAuthor);
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
            searchResult = dbContext.Authors.ToList();
        }

        private void CloseModalWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        public void Search()
        {
            var search = SearchText.ToLower();
            searchResult = dbContext.Authors.Where(s => (s.LastName.ToLower() + " " + s.FirstName.ToLower() + " " + s.Patronimyc.ToLower()).Contains(search)).ToList();
            Authors = searchResult;
            SignalChanged(nameof(Authors));
        }
    }


}

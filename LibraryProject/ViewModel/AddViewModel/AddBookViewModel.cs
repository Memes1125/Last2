using LibraryProject.Core;
using LibraryProject.DB;
using LibraryProject.Windows.AddBook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel.AddViewModel
{
    public class AddBookViewModel : BaseViewModel
    {
        public Library_DBContext dbContext = new Library_DBContext();

        public decimal FullPrice { get; set; }

        
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

        #region Properties
        public Book AddBookVM { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Author> Authors { get; set; }
        public List<Publisher> Publishers { get; set; }
        private Publisher selectedPublisher { get; set; }
        public DateTime SelectedDate { get; set; }
        public Publisher SelectedPublisher
        {
            get => selectedPublisher;
            set
            {
                selectedPublisher = value;
                SignalChanged();
            }
        }
        public List<Department> Departaments { get; set; }
        private Department selectedDepartament { get; set; }
        public Department SelectedDepartament
        {
            get => selectedDepartament;
            set
            {
                selectedDepartament = value;
                SignalChanged();
            }
        }


        public List<Shelf> Shelves { get; set; }
        private Shelf selectedShelf { get; set; }
        public Shelf SelectedShelf
        {
            get => selectedShelf;
            set
            {
                selectedShelf = value;
                SignalChanged();
            }
        }

        #region Selected Authors From Book Own and Selected Genres From Book
        public Author SelectedBookAuthor { get; set; }
        private ObservableCollection<Author> selectedBookAuthors = new ObservableCollection<Author>();
        public ObservableCollection<Author> SelectedBookAuthors
        {
            get => selectedBookAuthors;
            set
            {
                selectedBookAuthors = value;
                SignalChanged();
            }
        }
        public Genre SelectedBookGenre { get; set; }
        private ObservableCollection<Genre> selectedBookGenres = new ObservableCollection<Genre>();
        public ObservableCollection<Genre> SelectedBookGenres
        {
            get => selectedBookGenres;
            set
            {
                selectedBookGenres = value;
                SignalChanged();
            }
        }
        public ObservableCollection<BookAuthorCross> AllBookAuthors { get; set; }
        public ObservableCollection<BookGenreCross> AllBookGenres { get; set; }
        #endregion


        #endregion

        #region Commands
        public CustomCommand AddGenre { get; set; }
        public CustomCommand AddAuthor { get; set; }
        public CustomCommand DeleteGenre { get; set; }
        public CustomCommand DeleteAuthor { get; set; }
        public CustomCommand Save { get; set; }
        public CustomCommand Cancel { get; set; }
        #endregion



        public AddBookViewModel(Book book)
        {
            AllBookAuthors = new ObservableCollection<BookAuthorCross>();

            Genres = dbContext.Genres.ToList();
            Authors = dbContext.Authors.ToList();
            Publishers = dbContext.Publishers.ToList();
            Departaments = dbContext.Departments.ToList();
            Shelves = dbContext.Shelves.ToList();
            if (book == null)
            {
                AddBookVM = new Book { };
                SelectedDate = DateTime.Now;
            }

            else
            {
                AddBookVM = new Book
                {
                    Id = book.Id,
                    BookName = book.BookName,
                    Count = book.Count,
                    YearPublished = book.YearPublished,
                    Price = book.Price,
                    Departament = book.Departament,
                    DepartamentId = book.DepartamentId,
                    Publisher = book.Publisher,
                    PublisherId = book.PublisherId,
                    BookGenreCrosses = book.BookGenreCrosses,
                    BookAuthorCrosses = book.BookAuthorCrosses,
                    Shelf = book.Shelf,
                    ShelfId = book.ShelfId
                };
                SelectedDate = (DateTime)book.YearPublished;
                SelectedDepartament = Departaments.FirstOrDefault(s => s.Id == AddBookVM.DepartamentId);
                SelectedPublisher = Publishers.FirstOrDefault(s => s.Id == AddBookVM.PublisherId);
                SelectedShelf = Shelves.FirstOrDefault(s => s.Id == AddBookVM.ShelfId);

                RefreshAuthors(book);
                RefreshGenres(book);
            }

            AddGenre = new CustomCommand(() =>
            {
                if(book != null)
                {
                    AddGenreBook addGenreBook = new AddGenreBook(book, SelectedBookGenres = new ObservableCollection<Genre>());
                    addGenreBook.ShowDialog();
                    RefreshGenres(book);
                }
                else
                {
                    AddGenreBook addGenreBook = new AddGenreBook(book = null, SelectedBookGenres);
                    addGenreBook.ShowDialog();
                }
            });

            AddAuthor = new CustomCommand(() =>
            {
                if(book != null)
                {
                    AddAuthorBook addAuthorBook = new AddAuthorBook(book, SelectedBookAuthors = new ObservableCollection<Author>());
                    addAuthorBook.ShowDialog();
                    RefreshAuthors(book);
                }
                else
                {
                    AddAuthorBook addAuthorBook = new AddAuthorBook(book = null,SelectedBookAuthors);
                    addAuthorBook.ShowDialog();
                }
            });

            DeleteAuthor = new CustomCommand(() =>
            {
                if(book != null)
                {
                    BookAuthorCross bookAuthorCross = dbContext.BookAuthorCrosses.Where(s => s.AuthorId == SelectedBookAuthor.Id && s.BookId == book.Id).FirstOrDefault();
                    dbContext.BookAuthorCrosses.Remove(bookAuthorCross);
                    dbContext.SaveChanges();
                    RefreshAuthors(book);
                }
                else
                {
                   SelectedBookAuthors.Remove(SelectedBookAuthor);
                }
            });

            DeleteGenre = new CustomCommand(() =>
            {
                if(book != null)
                {
                    BookGenreCross bookGenreCross = dbContext.BookGenreCrosses.Where(s=> s.GenreId == SelectedBookGenre.Id && s.BookId == book.Id).FirstOrDefault();
                    dbContext.BookGenreCrosses.Remove(bookGenreCross);
                    dbContext.SaveChanges();
                    RefreshGenres(book);
                }
                else
                {
                    SelectedBookGenres.Remove(SelectedBookGenre);
                }
            });

            Save = new CustomCommand(() =>
            {
                AddBookVM.DepartamentId = SelectedDepartament.Id;
                AddBookVM.PublisherId = SelectedPublisher.Id;
                AddBookVM.ShelfId = SelectedShelf.Id;
                AddBookVM.YearPublished = SelectedDate;
                
                if (AddBookVM.Id == 0)
                {
                    dbContext.Books.Add(AddBookVM);
                    dbContext.SaveChanges();
                    BookAuthorCross bookAuthorCross = new BookAuthorCross();
                    BookGenreCross bookGenreCross = new BookGenreCross();
                    foreach (var item in SelectedBookAuthors)
                    {
                        bookAuthorCross.AuthorId = item.Id;
                        bookAuthorCross.BookId = AddBookVM.Id;
                        dbContext.BookAuthorCrosses.Add(bookAuthorCross);
                        dbContext.SaveChanges();
                    }
                    foreach (var item in SelectedBookGenres)
                    {
                        bookGenreCross.GenreId = item.Id;
                        bookGenreCross.BookId = AddBookVM.Id;
                        dbContext.BookGenreCrosses.Add(bookGenreCross);
                        dbContext.SaveChanges();
                    }
                    
                }
                else
                {
                    //dbContext.Entry(book).CurrentValues.SetValues(AddBookVM);
                    dbContext.Entry(AddBookVM).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                dbContext.SaveChanges(); 

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                        CloseModalWindow(window);
                }
            });

            Cancel = new CustomCommand(() =>
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.DataContext == this)
                        CloseModalWindow(window);
                }
            });
        }

        private void CloseModalWindow(object obj)
        {
            Window window = obj as Window;
            window.Close();
        }

        #region RefreshAuthors and RefreshGenres
        public void RefreshAuthors(Book book)
        {
            AllBookAuthors = new ObservableCollection<BookAuthorCross>();
            foreach (var item in dbContext.BookAuthorCrosses.Where(s => s.BookId == book.Id))
            {
                AllBookAuthors.Add(item);
            }
            SelectedBookAuthors = new ObservableCollection<Author>();
            foreach (var item in AllBookAuthors)
            {
                SelectedBookAuthors.Add(dbContext.Authors.FirstOrDefault(s => s.Id == item.AuthorId));
            }
        }
        
        

        public void RefreshGenres(Book book)
        {
            AllBookGenres = new ObservableCollection<BookGenreCross>();
            foreach (var item in dbContext.BookGenreCrosses.Where(s => s.BookId == book.Id))
            {
                AllBookGenres.Add(item);
            }
            SelectedBookGenres = new ObservableCollection<Genre>();
            foreach (var item in AllBookGenres)
            {
                SelectedBookGenres.Add(dbContext.Genres.FirstOrDefault(s => s.Id == item.GenreId));
            }
        }
        #endregion

    }
}

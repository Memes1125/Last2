using LibraryProject.Core;
using LibraryProject.DB;
using LibraryProject.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel
{
    public class BookViewModel : BaseViewModel
    {
        public Library_DBContext dbContext = new Library_DBContext();

        #region для пагинации
        public CustomCommand BackPage { get; set; }
        public CustomCommand ForwardPage { get; set; }
        int paginationPageIndex = 0;
        public int rows = 0;
        public int CountPages = 0;
        public List<string> ViewCountRows { get; set; }
        private string selectedViewCountRows;
        public string SelectedViewCountRows
        {
            get => selectedViewCountRows;
            set
            {
                selectedViewCountRows = value;
                paginationPageIndex = 0;
                Pagination();
                InitPagination();
            }
        }
        public List<Book> countForSearch { get; set; }
        public List<Book> CountForSearch
        {
            get => countForSearch;
            set
            {
                countForSearch = value;
                SignalChanged();
            }
        }
        private string searchCountRows;
        public string SearchCountRows
        {
            get => searchCountRows;
            set
            {
                searchCountRows = value;
                SignalChanged();
            }
        }

        private string pages;
        public string Pages
        {
            get => pages;
            set
            {
                pages = value;
                SignalChanged();
            }
        }
        #endregion

        private List<Book> books;
        public List<Book> Books
        {
            get => books;
            set
            {
                books = value;
                SignalChanged();
            }
        }
        public List<Book> FullBooks { get; set; }
        public List<BookGenreCross> Genres { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<BookAuthorCross> Authors { get; set; }
        public List<Shelf> ShelfFilter { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<Department> DepartmentFilter { get; set; }

        private List<Book> searchResult;

        private Shelf selectedShelfFilter { get; set; }
        public Shelf SelectedShelfFilter
        {
            get => selectedShelfFilter;
            set
            {
                selectedShelfFilter = value;
                Search();
            }
        }

        private Department selectedDepartmentFilter { get; set; }
        public Department SelectedDepartmentFilter
        {
            get => selectedDepartmentFilter;
            set
            {
                selectedDepartmentFilter = value;
                Search();
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

        public List<string> SearchType { get; set; }
        private string selectedSearchType;
        public string SelectedSearchType
        {
            get => selectedSearchType;
            set
            {
                selectedSearchType = value;
                Search();
            }
        }

        public List<string> SortType { get; set; }
        private string selectedSortType;
        public string SelectedSortType
        {
            get => selectedSortType;

            set
            {
                selectedSortType = value;
                //Sort();
            }
        }

        private string selectedOrderType;
        public List<string> OrderType { get; set; }
        public string SelectedOrderType
        {
            get => selectedOrderType;
            set
            {
                selectedOrderType = value;
                //Sort();
            }
        }

        private Book selectedBook { get; set; }
        public Book SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                SignalChanged();
            }
        }

        //public string BookGenres { get; set; }

        public void GetBook(Library_DBContext dBContext)
        {
            Books = dBContext.Books.ToList();
            Genres = dBContext.BookGenreCrosses.ToList();
            //Authors = dBContext.BookAuthorCrosses.ToList();
            ShelfFilter = dbContext.Shelves.ToList();
            Shelves = dBContext.Shelves.ToList();
            Publishers = dBContext.Publishers.ToList();
            DepartmentFilter = dBContext.Departments.ToList();
            SignalChanged("Books");
            foreach (var book in Books)
            {
                book.Publisher = Publishers.FirstOrDefault(s => s.Id == book.PublisherId);
                book.Departament = DepartmentFilter.FirstOrDefault(s => s.Id == book.DepartamentId);
                book.BookGenreCrosses = Genres.Where(s => s.BookId == book.Id).ToList();
                book.FullPrice = book.Price * book.Count;
                
                book.Authors = new List<Author>();
                book.Genres = new List<Genre>();
                #region Something
                AllBookAuthors = new ObservableCollection<BookAuthorCross>();
                foreach (var item in dbContext.BookAuthorCrosses.Where(s => s.BookId == book.Id))
                {
                    AllBookAuthors.Add(item);
                }
                foreach (var item in AllBookAuthors)
                {
                    if (book.Authors.Count != 0)
                        book.BookAuthor += $", ";
                    var author = dbContext.Authors.FirstOrDefault(s => s.Id == item.AuthorId);
                    book.Authors.Add(author);
                    book.BookAuthor += $"{author.LastName}" + $" {author.FirstName}";
                }


                AllBookGenres = new ObservableCollection<BookGenreCross>();
                foreach (var item in dbContext.BookGenreCrosses.Where(s => s.BookId == book.Id))
                {
                    AllBookGenres.Add(item);
                }
                foreach (var item in AllBookGenres)
                {
                    if (book.Genres.Count != 0)
                        book.BookGenre += $", ";
                    var genre = dbContext.Genres.FirstOrDefault(s => s.Id == item.GenreId);
                    book.Genres.Add(genre);
                    book.BookGenre += $"{genre.GenreName}";
                }
                #endregion
            }
            FullBooks = Books;
        }

        public ObservableCollection<BookAuthorCross> AllBookAuthors { get; set; }
        public ObservableCollection<BookGenreCross> AllBookGenres { get; set; }

        public CustomCommand EditBook { get; set; }
        public CustomCommand AddBook { get; set; }
        public CustomCommand DeleteBook { get; set; }

        public BookViewModel()
        {
            GetBook(dbContext);

            SearchType = new List<string>();
            SearchType.AddRange(new string[] { "Название", "Автор", "Цена", "Жанр", "Год публикации" });
            selectedSearchType = SearchType.First();

            DepartmentFilter.Add(new Department { DepartmentName = "Все типы" });
            DepartmentFilter.Reverse();
            selectedDepartmentFilter = DepartmentFilter.First();

            ShelfFilter.Add(new Shelf { ShelfNumber = "Все стеллажи" });
            ShelfFilter.Reverse();
            selectedShelfFilter = ShelfFilter.First();

            SortType = new List<string>();
            SortType.AddRange(new string[] { "Год издания", "Отмена", "Цена", "Остаток" });
            selectedSortType = SortType.First();

            OrderType = new List<string>();
            OrderType.AddRange(new string[] { "По возрастанию", "По убыванию", "По умолчанию" });
            selectedOrderType = OrderType.Last();

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "20", "все" });
            selectedViewCountRows = ViewCountRows.First();

            BackPage = new CustomCommand(() =>
            {
                if (Books == null)
                    return;
                if (paginationPageIndex > 0)
                    paginationPageIndex--;
                Pagination();
                InitPagination();
            });
            ForwardPage = new CustomCommand(() =>
            {
                if (searchResult == null)
                    return;
                int.TryParse(SelectedViewCountRows, out int rowsOnPage);
                if (rowsOnPage == 0)
                    return;
                int countPage = searchResult.Count() / rowsOnPage;
                CountPages = countPage;
                if (searchResult.Count() % rowsOnPage != 0)
                    countPage++;
                if (countPage > paginationPageIndex + 1)
                    paginationPageIndex++;
                Pagination();
                InitPagination();

            });


            AddBook = new CustomCommand(() =>
            {
                AddBookWindow addBook = new AddBookWindow(null);
                addBook.ShowDialog();
                Update();
               
            });

            EditBook = new CustomCommand(() =>
            {
                if (SelectedBook == null || SelectedBook.Id == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Вы не выбрали книгу!", "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
                else
                {
                    AddBookWindow addWindow = new AddBookWindow(SelectedBook);
                    addWindow.ShowDialog();
                    Update();
                    
                }
            });
            searchResult = dbContext.Books.ToList();
            SignalChanged("Books");

            InitPagination();
            Pagination();
        }

        private void Update()
        {
            dbContext = new Library_DBContext();
            GetBook(dbContext);
            SignalChanged(nameof(books));
        }

        public void Search()
        {
            var search = SearchText.ToLower();
            if (SelectedSearchType == "Название")
            {
                searchResult = dbContext.Books.Where(s => s.BookName.ToLower().Contains(search)).ToList();
            }
            else if (SelectedSearchType == "Автор")
            {
                searchResult.Clear();
                foreach (var book in FullBooks)
                {    
                    foreach (var author in book.Authors)
                    {
                        if ((author.LastName.ToLower() + " " + author.FirstName.ToLower() + " " + author.Patronimyc.ToLower()).Contains(search))
                        {
                            searchResult.Add(book);
                            break;
                        }
                    }
                }
            }
            else if (SelectedSearchType == "Цена")
            {
                searchResult = dbContext.Books.Where(s => s.Price.ToString().ToLower().Contains(search)).ToList();
            }
            else if (SelectedSearchType == "Жанр")
            {
                searchResult.Clear();
                foreach (var book in FullBooks)
                {
                    foreach (var genre in book.Genres)
                    {
                        if (genre.GenreName.ToLower().Contains(search))
                        {
                            searchResult.Add(book);
                            break;
                        }
                    }
                }
            }
            else if (SelectedSearchType == "Год публикации")
            {
                searchResult = dbContext.Books.Where(s => s.YearPublished.ToString().ToLower().Contains(search)).ToList();
            }
            if (SelectedDepartmentFilter.DepartmentName != "Все типы")
            {
                var a = searchResult;
                searchResult = a.Where(s => s.Departament.DepartmentName.Contains(SelectedDepartmentFilter.DepartmentName)).ToList();
            }

            if (SelectedShelfFilter.ShelfNumber != "Все стеллажи")
            {
                var a = searchResult;
                searchResult = a.Where(s => s.Shelf.ShelfNumber.Contains(SelectedShelfFilter.ShelfNumber)).ToList();
            }

            Books = searchResult;
            SignalChanged(nameof(Books));
            //Sort();
            InitPagination();
            Pagination();
        }

        //public void Sort()
        //{
        //    searchResult = dbContext.Books.ToList();
        //    if (SelectedOrderType == "По умолчанию")
        //        return;

            
        //    //paginationPageIndex = 0;
        //    Pagination();
        //}
        
        #region Методы пагинации
        private void InitPagination()
        {
            try
            {
                if (searchResult != null)
                {
                    SearchCountRows = $"Найдено записей: {Books.Count} из {searchResult.Count()}";
                }
                else
                    SearchCountRows = $"Ни одной записи не найдено";
            }
            catch
            {
                MessageBox.Show("Оишбка пагинации, стоит нажать на кнопку заново");
            }

        }

        public void Pagination()
        {
            int rowsOnPage = 0;
            try
            {
                if (!int.TryParse(SelectedViewCountRows, out rowsOnPage))
                {
                    Books = searchResult;
                }
                else
                {
                    Books = searchResult.Skip(rowsOnPage * paginationPageIndex)
                         .Take(rowsOnPage).ToList();
                }

                int.TryParse(SelectedViewCountRows, out rows);
                if (rows == 0)
                {
                    int all = 0;
                    all = searchResult.Count();
                    int result = all - all + 1;
                    rows = 1;
                    CountPages = (result - 1) / rows;
                }
                else
                {
                    CountPages = (searchResult.Count() - 1) / rows;
                }
                Pages = $"{paginationPageIndex + 1}/{CountPages + 1}";
            }
            catch
            {
                MessageBox.Show("Что-то не загрузилось, давайте ждать");
            }
        }
        #endregion
    }
}

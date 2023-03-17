using LibraryProject.Core;
using LibraryProject.DB;
using LibraryProject.Windows.AddAuthor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel
{
    public class AuthorViewModel: BaseViewModel
    {
        Library_DBContext dbContext = new Library_DBContext();

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

        public List<Author> authors;
        public List<Author> Authors 
        {
            get => authors;
            set
            {
                authors = value;
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

        public List<Author> searchResult { get; set; }

        public CustomCommand AddAuthor { get; set; }
        public CustomCommand EditAuthor { get; set; }
        public CustomCommand DeleteAuthor { get; set; }

        public void GetAutors(Library_DBContext dbContext)
        {
            Authors = dbContext.Authors.ToList();
        }

        public AuthorViewModel()
        {
            GetAutors(dbContext);
            AddAuthor = new CustomCommand(() =>
            {
                AddAuthor addAuthor = new AddAuthor();
                addAuthor.ShowDialog();
                Update();
            });

            EditAuthor = new CustomCommand(() =>
            {
                AddAuthor addAuthor = new AddAuthor(SelectedAuthor);
                addAuthor.ShowDialog();
                Update();
            });

            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "20", "все" });
            selectedViewCountRows = ViewCountRows.First();

            BackPage = new CustomCommand(() =>
            {
                if (Authors == null)
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
            searchResult = dbContext.Authors.ToList();
            SignalChanged("Authors");
            InitPagination();
            Pagination();
        }
        private void Update()
        {
            dbContext = new Library_DBContext();
            Authors = new List<Author>(dbContext.Authors);
            SignalChanged(nameof(authors));
        }
        public void Search()
        {
            var search = SearchText.ToLower();
            searchResult = dbContext.Authors.Where(s => (s.LastName.ToLower() +" "+ s.FirstName.ToLower() + " " + s.Patronimyc.ToLower()).Contains(search)).ToList();
            Authors = searchResult;
            SignalChanged(nameof(Authors));
            InitPagination();
            Pagination();
        }

        #region Методы пагинации
        private void InitPagination()
        {
            try
            {
                if (searchResult != null)
                {
                    SearchCountRows = $"Найдено записей: {Authors.Count} из {searchResult.Count()}";
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
                    Authors = searchResult;
                }
                else
                {
                    Authors = searchResult.Skip(rowsOnPage * paginationPageIndex)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Что-то не загрузилось, давайте ждать");
            }
        }
        #endregion
    }
}

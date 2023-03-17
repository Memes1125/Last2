using LibraryProject.Core;
using LibraryProject.DB;
using LibraryProject.Windows.AddPublisher;
using LibraryProject.Windows.AddShelf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel
{
    public class PublisherViewModel: BaseViewModel
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

        public List<Publisher> publishers;
        public List<Publisher> Publishers
        {
            get => publishers;
            set
            {
                publishers = value;
                SignalChanged(nameof(publishers));
            }
        }

        public List<Publisher> searchResult { get; set; }

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

        private Publisher selectedPublisher { get; set; }
        public Publisher SelectedPublisher
        {
            get
            {
                return selectedPublisher;
            }
            set
            {
                selectedPublisher = value;
                SignalChanged();
            }
        }

        public CustomCommand AddPublisher { get; set; }
        public CustomCommand EditPublisher { get; set; }
        public CustomCommand DeletePublisher { get; set; }

        public PublisherViewModel()
        {
            Publishers = dbContext.Publishers.ToList();

            AddPublisher = new CustomCommand(() =>
            {
                AddPublisher addPublisher = new AddPublisher();
                addPublisher.ShowDialog();
                Update();
            });

            EditPublisher = new CustomCommand(() =>
            {
                if (SelectedPublisher == null)
                {
                    MessageBox.Show("Вы не выбрали издателя");
                }
                else
                {
                    AddPublisher addPublisher = new AddPublisher(SelectedPublisher);
                    addPublisher.ShowDialog();
                    Update();
                }
            });
            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "20", "все" });
            selectedViewCountRows = ViewCountRows.First();

            BackPage = new CustomCommand(() =>
            {
                if (Publishers == null)
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
            searchResult = dbContext.Publishers.ToList();
            SignalChanged("Publishers");
            InitPagination();
            Pagination();
        }
        private void Update()
        {
            dbContext = new Library_DBContext();
            Publishers = new List<Publisher>(dbContext.Publishers);
            SignalChanged(nameof(publishers));
        }
        public void Search()
        {
            var search = SearchText.ToLower();
            searchResult = dbContext.Publishers.Where(s => s.PublisherLocation.ToLower().Contains(search) || s.PublisherName.ToLower().Contains(search)).ToList();
            Publishers = searchResult;
            SignalChanged(nameof(Publishers));
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
                    SearchCountRows = $"Найдено записей: {Publishers.Count} из {searchResult.Count()}";
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
                    Publishers = searchResult;
                }
                else
                {
                    Publishers = searchResult.Skip(rowsOnPage * paginationPageIndex)
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
                MessageBox.Show($"Что-то не загрузилось, давайте ждать");
            }
        }
        #endregion
    }
}

﻿using LibraryProject.Core;
using LibraryProject.DB;
using LibraryProject.Windows.AddShelf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel
{
    public class ShelfViewModel: BaseViewModel
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

        private List<Shelf> shelves;
        public List<Shelf> Shelves
        {
            get => shelves;
            set
            {
                shelves = value;
                SignalChanged(nameof(shelves));
            }
        }

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

        public CustomCommand AddShelf { get; set; }
        public CustomCommand EditShelf { get; set; }
        public CustomCommand DeleteShelf { get; set; }

        public List<Shelf> searchResult { get; set; }

        public ShelfViewModel()
        {
            Shelves = dbContext.Shelves.ToList();
            AddShelf = new CustomCommand(() =>
            {
                AddShelf addShelf = new AddShelf();
                addShelf.ShowDialog();
                Update();
            });

            EditShelf = new CustomCommand(() =>
            {
                if (SelectedShelf == null || SelectedShelf.Id == 0)
                    MessageBox.Show("Вы не выбрали стелаж!");
                AddShelf addShelf = new AddShelf(SelectedShelf);
                addShelf.ShowDialog();
                Update();
            });
            ViewCountRows = new List<string>();
            ViewCountRows.AddRange(new string[] { "20", "все" });
            selectedViewCountRows = ViewCountRows.First();

            BackPage = new CustomCommand(() =>
            {
                if (Shelves == null)
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
            searchResult = dbContext.Shelves.ToList();
            SignalChanged("Shelves");
            Pagination();
            InitPagination();
        }

        private void Update()
        {
            dbContext = new Library_DBContext();
            Shelves = new List<Shelf>(dbContext.Shelves);
            SignalChanged(nameof(shelves));
        }    

        public void Search()
        {
            var search = SearchText.ToLower();
            searchResult = dbContext.Shelves.Where(s => s.ShelfNumber.ToLower().Contains(search)).ToList();
            Shelves = new List<Shelf>(searchResult);
            SignalChanged(nameof(Shelves));

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
                    SearchCountRows = $"Найдено записей: {Shelves.Count} из {searchResult.Count()}";
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
                    Shelves = searchResult;
                }
                else
                {
                    Shelves = searchResult.Skip(rowsOnPage * paginationPageIndex)
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

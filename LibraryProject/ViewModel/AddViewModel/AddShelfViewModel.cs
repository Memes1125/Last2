using LibraryProject.Core;
using LibraryProject.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryProject.ViewModel.AddViewModel
{
    public class AddShelfViewModel: BaseViewModel
    {
        Library_DBContext dbContext = new Library_DBContext();

        public CustomCommand SaveShelf { get; set; }

        public CustomCommand Cancel { get; set; }

        public Shelf AddShelfVM { get; set; }

        public AddShelfViewModel(Shelf shelf)
        {
            if (shelf == null)
            {
                AddShelfVM = new Shelf { };
            }
            else
            {
                AddShelfVM = new Shelf
                {
                    Id = shelf.Id,
                    ShelfNumber = shelf.ShelfNumber
                };
            }

            SaveShelf = new CustomCommand(() =>
            {
                if (AddShelfVM.Id == 0)
                {
                    dbContext.Shelves.Add(AddShelfVM);
                }
                else
                {
                    dbContext.Update(AddShelfVM);
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
    }

}

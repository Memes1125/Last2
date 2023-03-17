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
    public class AddAuthorViewModel: BaseViewModel
    {
        Library_DBContext dbContext = new Library_DBContext();
        public CustomCommand SaveAuthor { get; set; }
        public CustomCommand Cancel { get; set; }

        public Author AddAuthorVM { get; set; }

        public AddAuthorViewModel(Author author)
        {
            if (author == null)
            {
                AddAuthorVM = new Author { };
            }
            else
            {
                AddAuthorVM = new Author
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Patronimyc = author.Patronimyc
                };
            }

            SaveAuthor = new CustomCommand(() =>
            {
                if (AddAuthorVM.FirstName == "")
                {
                    MessageBox.Show("Не введено имя автора");
                    return;
                }
                if (AddAuthorVM.Id == 0)
                {
                    dbContext.Authors.Add(AddAuthorVM);
                }
                else
                {
                    dbContext.Update(AddAuthorVM);
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

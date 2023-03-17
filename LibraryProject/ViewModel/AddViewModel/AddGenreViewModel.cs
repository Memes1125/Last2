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
    public class AddGenreViewModel: BaseViewModel
    {
        Library_DBContext dbContext = new Library_DBContext();
        public Genre AddGenreVM { get; set; }

        public CustomCommand SaveGenre { get; set; }
        public CustomCommand Cancel { get; set; }

        public AddGenreViewModel(Genre genre)
        {
            if (genre == null)
            {
                AddGenreVM = new Genre { GenreName = "Новый жанр" };
            }
            else
            {
                AddGenreVM = new Genre
                {
                    Id = genre.Id,
                    GenreName = genre.GenreName
                };
            }

            SaveGenre = new CustomCommand(() =>
            {
                if (AddGenreVM.GenreName == "")
                {
                    MessageBox.Show("Введите название жанра");
                    return;
                }
                if (AddGenreVM.Id == 0)
                {
                    dbContext.Genres.Add(AddGenreVM);
                }
                else
                {
                    dbContext.Update(AddGenreVM);
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

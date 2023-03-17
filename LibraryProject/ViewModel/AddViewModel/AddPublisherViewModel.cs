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
    public class AddPublisherViewModel: BaseViewModel
    {
        Library_DBContext dbContext = new Library_DBContext();
        public CustomCommand SavePublisher { get; set; }
        public CustomCommand Cancel { get; set; }

        public Publisher AddPublisherVM { get; set; }

        public AddPublisherViewModel(Publisher publisher)
        {
            if (publisher == null)
            {
                AddPublisherVM = new Publisher { };
            }
            else
            {
                AddPublisherVM = new Publisher
                {
                    Id = publisher.Id,
                    PublisherLocation = publisher.PublisherLocation,
                    PublisherName = publisher.PublisherName
                };
            }

            SavePublisher = new CustomCommand(() =>
            {
                if (AddPublisherVM.PublisherName == "")
                {
                    MessageBox.Show("Не введено название редакции");
                    return;
                }
                if (AddPublisherVM.PublisherLocation == "")
                {
                    MessageBox.Show("Не введен город редакции");
                    return;
                }
                if (AddPublisherVM.Id == 0)
                {
                    dbContext.Publishers.Add(AddPublisherVM);
                }
                else
                {
                    dbContext.Update(AddPublisherVM);
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

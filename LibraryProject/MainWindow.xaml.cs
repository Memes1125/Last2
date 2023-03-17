using LibraryProject.DB;
using LibraryProject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Library_DBContext dbContext = new Library_DBContext();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            //Import();
            
        }
        public List<BookGenreCross> booksgenre = new List<BookGenreCross>();

        public void Import()
        {
            string path = @"C:\Users\Student\Desktop\LibraryProject-device23branch\LibraryProject\excel DB.csv";//
            var rows = File.ReadAllLines(path);

            //dbContext.Genres.Add(new Genre { GenreName = "Общеобразовательное" });
            //dbContext.Genres.Add(new Genre { GenreName = "Роман" });
            //dbContext.Genres.Add(new Genre { GenreName = "Роман в стихах" });


            //for (int i = 1; i < rows.Length; i++)
            //{
            //    var cols = rows[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            //    var shelf = dbContext.Shelves.First(s => s.ShelfNumber == cols[8]);
            //    var dep = dbContext.Departments.First(s => s.DepartmentName == cols[4]);
            //    var pubname = cols[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    var pub = dbContext.Publishers.First(s => s.PublisherName == pubname[0]);

            //    var date = cols[2];
            //    date = $"01.01.{cols[2]}";
            //    books.Add(new Book
            //    {
            //        BookName = cols[0],
            //        Count = int.Parse(cols[6]),
            //        ShelfId = shelf.Id,
            //        Shelf = shelf,
            //        Departament = dep,
            //        DepartamentId = dep.Id,
            //        Publisher = pub,
            //        PublisherId = pub.Id,
            //        YearPublished = DateTime.Parse((date)),
            //        Price = 0,
            //    }) ; 


            //    dbContext.Books.AddRange(books);

            //}
            //dbContext.SaveChanges();

            //var bookes = dbContext.Books.ToList();
            //var authores = dbContext.Authors.ToList();
            //for (int i = 1; i < rows.Length; i++)
            //{
            //    var cols = rows[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //    var col = cols[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    var pubname = cols[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    var book = dbContext.Books.First(s => s.BookName == cols[0] && s.Publisher.PublisherName == pubname[0]);
            //    for (int a = 0; a < col.Length; a++)
            //    {
            //        var fio = col[a].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //        var author = dbContext.Authors.First(s => s.LastName == fio[2]);
            //        var ab = false;
            //        var bok = booksauth.FirstOrDefault(s => s.AuthorId == author.Id && s.BookId == book.Id);
            //        if (bok != null)
            //        {
            //            ab = true;
            //        }
            //        if (!ab)
            //        {
            //            booksauth.Add(new BookAuthorCross
            //            {
            //                Book = book,
            //                BookId = book.Id,
            //                Author = author,
            //                AuthorId = author.Id,
            //            });;

            //        }


            //dbContext.BookAuthorCrosses.Add(new BookAuthorCross
            //        {
            //             Book = book,
            //             BookId = book.Id,
            //             Author = author,
            //             AuthorId = author.Id

            //        });
            //    }
            //    }
            //}
            //dbContext.BookAuthorCrosses.AddRange(booksauth);
            // dbContext.SaveChanges();



            var bookes = dbContext.Books.ToList();
            var genres = dbContext.Genres.ToList();
            for (int i = 1; i < rows.Length; i++)
            {
                var cols = rows[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var col = cols[5].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var pubname = cols[7].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var book = dbContext.Books.First(s => s.BookName == cols[0] && s.Publisher.PublisherName == pubname[0]);
                for (int a = 0; a < col.Length; a++)
                {
                    var genre = dbContext.Genres.First(s => s.GenreName == col[a]);
                    var ab = false;
                    var bok = booksgenre.FirstOrDefault(s => s.GenreId == genre.Id && s.BookId == book.Id);
                    if (bok != null)
                    {
                        ab = true;
                    }
                    if (!ab)
                    {
                        booksgenre.Add(new BookGenreCross
                        {
                            Book = book,
                            BookId = book.Id,
                           Genre = genre,
                           GenreId = genre.Id,
                        }); ;

                    }
                }
            }
            //dbContext.BookGenreCrosses.AddRange(booksgenre);
            //dbContext.SaveChanges();



        }
    }
}

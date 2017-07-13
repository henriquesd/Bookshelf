using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BookshelfData
{
    public class BookManager
    {
        public string filePath = @"C:\Bookshelf\books.xml";
        public string folderPath = @"C:\Bookshelf";

        public List<Book> books;
        public List<KeyValuePair<string, string>> ValidationErrors { get; set; }

        public BookManager()
        {
            books = GetBooks(null);

            if (books == null)
            {
                books = new List<Book>();
                books = CreateInitialBooks();
            }

            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public List<Book> GetBooks(string searchField = "")
        {
            try
            {
                List<Book> booksLoad = new List<Book>();
                XmlDocument xmlDoc = new XmlDocument();

                if (File.Exists(filePath))
                {
                    xmlDoc.Load(filePath);
                }
                else
                {
                    CreateInitialBooks();
                }

                XmlNodeList nodelist = xmlDoc.SelectNodes("books/book");

                foreach (XmlNode node in nodelist)
                {
                    Book book = new Book();

                    book.Id = Convert.ToInt32(node.SelectSingleNode("id").InnerText);
                    book.Name = Convert.ToString(node.SelectSingleNode("name").InnerText);
                    book.Author = Convert.ToString(node.SelectSingleNode("author").InnerText);
                    book.Price = Convert.ToDecimal(node.SelectSingleNode("price").InnerText);

                    booksLoad.Add(book);
                }

                if (!String.IsNullOrEmpty(searchField))
                {
                    booksLoad = booksLoad.FindAll(b => b.Name.ToLower().Contains(searchField.ToLower())
                  || b.Author.ToLower().Contains(searchField.ToLower())
                  || b.Price.ToString().Contains(searchField.ToLower()));
                }

                return booksLoad;
            }
            catch (Exception e)
            {
                throw new Exception("Error on load books: " + e);
            }
        }

        public Book GetBook(int id)
        {
            try
            {
                Book entity = null;

                entity = books.Find(p => p.Id == id);

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on load book: " + ex);
            }
        }

        public bool Update(Book entity)
        {
            try
            {
                bool ret = false;

                ret = Validate(entity);

                if (ret)
                {

                    string file = filePath;
                    XElement xml = XElement.Load(file);
                    IEnumerable<XElement> elements = xml.Elements();

                    foreach (var item in elements)
                    {
                        if (item.Element("id").Value == entity.Id.ToString())
                        {
                            item.Element("name").Value = entity.Name;
                            item.Element("author").Value = entity.Author;
                            item.Element("price").Value = entity.Price.ToString();
                        }
                    }
                    xml.Save(file);
                }

                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on update record on database: " + ex);
            }
        }

        public bool Delete(Book entity)
        {
            try
            {
                string file = filePath;
                XElement xml = XElement.Load(file);
                IEnumerable<XElement> elements = xml.Elements();

                foreach (var item in elements)
                {
                    if (item.Element("id").Value == entity.Id.ToString())
                    {
                        item.Remove();
                    }
                }
                xml.Save(file);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on delete record: " + ex);
            }
        }

        public bool Validate(Book entity)
        {
            try
            {
                ValidationErrors.Clear();

                if (string.IsNullOrEmpty(entity.Name))
                {
                    ValidationErrors.Add(new KeyValuePair<string, string>("Book", "The name of the book must be informed."));
                }

                if (!string.IsNullOrEmpty(entity.Author))
                {
                    if ((entity.Author.Length < 2) || (entity.Author.Length > 250))
                    {
                        ValidationErrors.Add(new KeyValuePair<string, string>("Author's Name", "The autor's name must be greater than 2 characters and less than 250 characters."));
                    }

                    if (!entity.Author.Any(char.IsLetter))
                    {
                        ValidationErrors.Add(new KeyValuePair<string, string>("Author's Name", "It's not possible inform just number in the field Author's Name."));
                    }
                }
                else
                {
                    ValidationErrors.Add(new KeyValuePair<string, string>("Author's Name", "The autor's name must be informed."));
                }

                if (entity.Price == 0)
                {
                    ValidationErrors.Add(new KeyValuePair<string, string>("Price", "The Price of the book must be higher than 0."));
                }

                if (!string.IsNullOrEmpty(entity.Name))
                {
                    if ((entity.Name.Length < 2) || (entity.Name.Length > 250))
                    {
                        ValidationErrors.Add(new KeyValuePair<string, string>("Name", "The name length must be higher than 2 characters and lower than 250 characters."));
                    }
                }

                return (ValidationErrors.Count == 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error on validate: " + ex);
            }
        }

        public bool Insert(Book entity)
        {
            try
            {
                bool ret = false;

                ret = Validate(entity);

                if (ret)
                {
                    int lastId = (from b in books
                                  orderby b.Id descending
                                  select b.Id).First();

                    int newId = lastId + 1;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XmlNode root = doc.DocumentElement;
                    XmlNode newNode = root.SelectSingleNode("descendant::books");

                    doc.AppendChild(root);

                    XmlElement book = doc.CreateElement("book");
                    root.AppendChild(book);

                    XmlElement id = doc.CreateElement("id");
                    book.AppendChild(id);
                    id.InnerText = Convert.ToString(newId);

                    XmlElement name = doc.CreateElement("name");
                    book.AppendChild(name);
                    name.InnerText = Convert.ToString(entity.Name);

                    XmlElement author = doc.CreateElement("author");
                    book.AppendChild(author);
                    author.InnerText = Convert.ToString(entity.Author);

                    XmlElement price = doc.CreateElement("price");
                    book.AppendChild(price);
                    price.InnerText = Convert.ToString(entity.Price);

                    doc.Save(filePath);

                    return true;
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on insert record: " + ex);
            }
        }

        protected List<Book> CreateInitialBooks()
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlElement root = doc.CreateElement("books");
                doc.AppendChild(root);

                XmlProcessingInstruction pi = doc.CreateProcessingInstruction(@"xml", "version=\"1.0\" encoding=\"utf-8\"");

                doc.InsertBefore(pi, root);

                books = new List<Book>();

                books.Add(new Book()
                {
                    Id = 1,
                    Name = "The Flesh and The Spirit",
                    Author = "Witness Lee",
                    Price = Convert.ToDecimal(9.59)
                });
                books.Add(new Book()
                {
                    Id = 2,
                    Name = "Humility: The Beauty of Holiness",
                    Author = "Andrew Murray",
                    Price = Convert.ToDecimal(4.05)
                });
                books.Add(new Book()
                {
                    Id = 3,
                    Name = "Pet Sematary",
                    Author = "Stephen King",
                    Price = Convert.ToDecimal(15.35)
                });
                books.Add(new Book()
                {
                    Id = 4,
                    Name = "The Hobbit",
                    Author = "J. R. R. Tolkien",
                    Price = Convert.ToDecimal(35)
                });
                books.Add(new Book()
                {
                    Id = 5,
                    Name = "The Lord of The Rings",
                    Author = "J. R. R. Tolkien",
                    Price = Convert.ToDecimal(45.49)
                });
                books.Add(new Book()
                {
                    Id = 6,
                    Name = "How to Win Friends and Influence People",
                    Author = "Dale Carnegie",
                    Price = Convert.ToDecimal(44.12)
                });
                books.Add(new Book()
                {
                    Id = 7,
                    Name = "The Godfather",
                    Author = "Mario Puzo",
                    Price = Convert.ToDecimal(29.68)
                });

                foreach (var item in books)
                {
                    XmlElement book = doc.CreateElement("book");
                    root.AppendChild(book);

                    XmlElement id = doc.CreateElement("id");
                    book.AppendChild(id);
                    id.InnerText = Convert.ToString(item.Id);

                    XmlElement name = doc.CreateElement("name");
                    book.AppendChild(name);
                    name.InnerText = Convert.ToString(item.Name);

                    XmlElement author = doc.CreateElement("author");
                    book.AppendChild(author);
                    author.InnerText = Convert.ToString(item.Author);

                    XmlElement price = doc.CreateElement("price");
                    book.AppendChild(price);
                    price.InnerText = Convert.ToString(item.Price);
                }

                string folder = folderPath;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                doc.Save(filePath);


                return books;
            }
            catch (Exception ex)
            {
                throw new Exception("Error on load initial records: " + ex);
            }
        }

        public List<Book> RestoreDefault()
        {
            try
            {
                List<Book> booksLoad = new List<Book>();
                XmlDocument xmlDoc = new XmlDocument();

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    CreateInitialBooks();
                }
                else
                {
                    CreateInitialBooks();
                }

                XmlNodeList nodelist = xmlDoc.SelectNodes("books/book");

                foreach (XmlNode node in nodelist)
                {
                    Book book = new Book();

                    book.Id = Convert.ToInt32(node.SelectSingleNode("id").InnerText);
                    book.Name = Convert.ToString(node.SelectSingleNode("name").InnerText);
                    book.Author = Convert.ToString(node.SelectSingleNode("author").InnerText);
                    book.Price = Convert.ToDecimal(node.SelectSingleNode("price").InnerText);

                    booksLoad.Add(book);
                }

                return booksLoad;
            }
            catch (Exception e)
            {
                throw new Exception("Error on restore records: " + e);
            }
        }
    }
}

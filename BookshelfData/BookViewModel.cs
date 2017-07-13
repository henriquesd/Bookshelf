using BookshelfCommon;
using System;
using System.Collections.Generic;

namespace BookshelfData
{
    public class BookViewModel : ViewModelBase
    {
        BookManager bookManager = new BookManager();

        public BookViewModel() : base()
        {
            Books = new List<Book>();
            searchField = "";
            Entity = new Book();
        }

        public List<Book> Books { get; set; }
        public Book Entity { get; set; }
        public string searchField { get; set; }

        protected override void Init()
        {
            Books = new List<Book>();
            Entity = new Book();

            base.Init();
        }

        public override void HandleRequest()
        {
            switch (EventCommand.ToLower())
            {
                case "list":
                case "search":
                    Get(searchField);
                    break;
                case "add":
                    Add();
                    break;
                case "edit":
                    Edit();
                    break;
                case "delete":
                    ResetSearch();
                    Delete();
                    break;
                case "save":
                    Save();
                    Get();
                    break;
                case "cancel":
                    ListMode();
                    Get();
                    break;
                case "resetsearch":
                    ResetSearch();
                    Get();
                    break;
                case "restore":
                    RestoreDefault();
                    Get();
                    break;
            }
        }

        protected override void Add()
        {
            IsValid = true;

            Entity = new Book();
            Entity.Name = String.Empty;
            Entity.Author = String.Empty;
            Entity.Price = 0;

            base.Add();
        }

        protected override void Edit()
        {
            BookManager bookManager = new BookManager();

            Entity = bookManager.GetBook(Convert.ToInt32(EventArgument));

            base.Edit();
        }

        protected override void Save()
        {
            BookManager bookManager = new BookManager();

            if (Mode == "Add")
            {
                bookManager.Insert(Entity);
            }
            else
            {
                bookManager.Update(Entity);
            }

            ValidationErrors = bookManager.ValidationErrors;

            base.Save();
        }

        protected override void Delete()
        {
            BookManager bookManager = new BookManager();

            Entity = new Book();

            Entity.Id = Convert.ToInt32(EventArgument);

            bookManager.Delete(Entity);

            Get();

            base.Delete();
        }

        protected override void ResetSearch()
        {
            searchField = "";

            base.ResetSearch();
        }

        protected override void Get(string searchField = "")
        {
            BookManager bookManager = new BookManager();

            Books = bookManager.GetBooks(searchField);

            base.Get();
        }

        protected override void RestoreDefault()
        {
            BookManager bookManager = new BookManager();

            Books = bookManager.RestoreDefault();

            base.Get();
        }
    }
}

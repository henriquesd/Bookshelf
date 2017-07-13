using BookshelfData;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bookshelf.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            BookViewModel vm = new BookViewModel();

            vm.HandleRequest();

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(BookViewModel vm)
        {
            vm.IsValid = ModelState.IsValid;
            vm.HandleRequest();

            if (vm.IsValid)
            {
                ModelState.Clear();
            }
            else
            {
                foreach (KeyValuePair<string, string> item in vm.ValidationErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }
            }

            return View(vm);
        }
    }
}
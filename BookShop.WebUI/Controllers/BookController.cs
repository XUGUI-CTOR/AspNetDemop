using BookShop.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace BookShop.WebUI.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository repository;

        public BookController(IBookRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        // GET: Book
        public ActionResult List()
        {
            return View(repository.Books);
        }

        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);
        }
    }
}
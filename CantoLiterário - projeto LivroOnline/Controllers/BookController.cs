using CantoLiterário___projeto_LivroOnline.Context;
using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Services.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace CantoLiterário___projeto_LivroOnline.Controllers
{
    public class BookController : Controller
    {
        public UserManager<User> _userManager { get; set; }
        public SignInManager<User> _signInManager { get; set; }
        public AppDbContext _context { get; set; }
        public IMapper _mapper { get; set; }

		public BookController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_mapper = mapper;
		}

		public IActionResult Index()
        {
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [Authorize]
        public IActionResult CreateBook()
        {
            ViewBag.BookGenders = new SelectList(_context.Genders.ToList().OrderBy(n => n.Name), "GendersId", "Name");
            return View();
        }

        [HttpGet]
        [Route("Livros/{titulo}")]
        public IActionResult BookPage([FromRoute] string titulo)
            {
            var book = _context.Book.Where(book => book.Name == titulo).Where(p => p.Publicado == 1).Include(u => u.Author).Include(c => c.bookContents).Include(g => g.Genders).FirstOrDefault();
            if (book == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(book);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBook(BookDTO book, IFormFile img)
        {

            ModelState.Remove("img");
            if (ModelState.IsValid)
            {

                if (img != null)
                {
                    Regex regex = new Regex("[^a-zA-Z0-9]");
                    string name = regex.Replace(book.Name, "");
                    name += ".png";
                    if (img.ContentType.ToLower().StartsWith("image/"))
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens", "capas", name);
                        book.ImgUrl = name;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                    }
                }
                book.AuthorId = _userManager.GetUserId(User);

                var bookDb = _mapper.Map<Book>(book);

                _context.Book.Add(bookDb);
                _context.SaveChanges();
            }

            return RedirectToAction("CreateBook");

        }

        [Route("LivrosUsuario")]
        [Authorize]
        public IActionResult UserLibBook(User user)
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUserBooks()
        {
            string id = _userManager.GetUserId(User);
            var book = _context.Book.Where(a => a.AuthorId == id).Where(p => p.Publicado == 1).Select(b => new BookDTO { Name = b.Name, BookId = b.BookId, ImgUrl = b.ImgUrl,Publicado =  b.Publicado }).ToList();
            return PartialView("_BooksUserPartial", book);
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetUserBooksSketchs()
        {
            string id = _userManager.GetUserId(User);
            var book = _context.Book.Where(a => a.AuthorId == id).Where(p => p.Publicado == 0).Select(b => new BookDTO { Name = b.Name, BookId = b.BookId, ImgUrl = b.ImgUrl,Publicado = b.Publicado }).ToList();
            return PartialView("_BooksUserPartial", book);
        }
        [Authorize]
        public IActionResult DeleteByIdBook(int bookId)
        {
            Book book = _context.Book.Where(b => b.BookId == bookId).FirstOrDefault();
            string userId = _userManager.GetUserId(User);
            if (userId == book?.AuthorId)
            {
                _context.Book.Remove(book);
                _context.SaveChanges();
            }
            return RedirectToAction("GetUserBooks");
        }



        [Route("Livros/Edit/{titulo}")]
        [Authorize]
        public IActionResult EditBookPage([FromRoute] string titulo)
        {
            var userId = _userManager.GetUserId(User);
            var book = _context.Book.Where(a => a.AuthorId == userId).Where(b => b.Name == titulo).Include(a => a.Author).Include(c => c.bookContents).FirstOrDefault();
            if (book != null)
            {
                BookDTO bookDto = _mapper.Map<BookDTO>(book);
                return View(bookDto);
            }
            return RedirectToAction("Error", "Home");
        }
        [Authorize]
        public void AddChap(int BookId)
        {
            var book = _context.Book.Where(u => u.AuthorId == _userManager.GetUserId(User) && u.BookId == BookId).Include(a => a.bookContents).FirstOrDefault();
            if (book != null)
            {
                BookContent bc = new()
                {
                    Title = "Sem Titulo " + (book.bookContents?.Count + 1),
                    Date = DateTime.Now,
                    BookId = BookId
                };

                _context.BookContents.Add(bc);
                _context.SaveChanges();
            }

        }
        [Authorize]
        public void RemoveChap(int BookId)
        {
            var bookContent = _context.BookContents.Where(u => u.BookContentId == BookId)
                .Include(a => a.Book)
                    .ThenInclude(a => a.Author)
                        .Where(c => c.Book.AuthorId == _userManager.GetUserId(User)).FirstOrDefault();

            if (bookContent != null)
            {
                _context.BookContents.Remove(bookContent);
                _context.SaveChanges();
            }
        }
        [Authorize]
        public IActionResult PublishBook(int bookId)
        {
            var book = _context.Book.Where(b => b.BookId == bookId).FirstOrDefault();
            if (_userManager.GetUserId(User) == book?.AuthorId)
            {
                book.Publicado = 1;
                _context.Update(book);
                _context.SaveChanges();
            }
            return RedirectToAction("EditBookPage", "Book", new { titulo = book?.Name });
        }
        [Authorize]
        public IActionResult UnPublishBook(int bookId)
        {
            var book = _context.Book.Where(b => b.BookId == bookId).FirstOrDefault();
            if (_userManager.GetUserId(User) == book?.AuthorId)
            {
                book.Publicado = 0;
                _context.Update(book);
                _context.SaveChanges();
            }
            return RedirectToAction("EditBookPage", "Book", new { titulo = book?.Name });
        }
        [Authorize]
        [Route("Livros/Edit/{titulo}/Write/{contentId}")]
        public IActionResult WritePage([FromRoute] string titulo, [FromRoute] int contentId)
        {
            var content = _context.BookContents.Where(c => c.BookContentId == contentId)
                .Include(b => b.Book)
                    .ThenInclude(a => a.Author)
                        .Include(c => c.Lines)
                            .FirstOrDefault();

            if (content?.Book.AuthorId == _userManager.GetUserId(User) && content != null)
            {
                ContentDTO bookContent = new()
                {
                    BookContentId = content.BookContentId,
                    Book = new()
                    {
                        BookId = content.Book.BookId,
                        Name = content.Book.Name
                    },
                    Title = content?.Title,
                    Lines = content?.Lines,
                };
                return View(bookContent);
            }
            return RedirectToAction("Error", "Home");
        }

        [Authorize]
        public String SaveWritePage(ContentDTO bookContent)
        {
            var book = _context.BookContents.Where(i => i.BookContentId == bookContent.BookContentId).Include(b => b.Book).ThenInclude(a => a.Author).ToList();
            if (book != null)
            {
                if (book[0].Book.AuthorId == _userManager.GetUserId(User))
                {
                    var bc = _context.BookContents.Where(i => i.BookContentId == bookContent.BookContentId).Include(c => c.Lines).FirstOrDefault();
                    bc.Title = bookContent.Title;
                    bc.Lines = bookContent.Lines;
                    _context.BookContents.Update(bc);
                    _context.SaveChanges();
                    return "Sucesso";
                }

            }
            return "Error";
        }


        [Route("Livros/{idTitulo}/capitulo/{capitulo}")]
        public IActionResult ReadPage([FromRoute] int idTitulo, [FromRoute] string capitulo)
        {
            BookContent bookc = _context.BookContents.Include(b => b.Book)
                .ThenInclude(a => a.Author)
                    .Where(b => b.Book.BookId == idTitulo)
                    .Where(n => n.Title == capitulo)
                 .Include(c => c.comments)
                    .ThenInclude(a => a.User)
                 .Include(c => c.comments)
                    .ThenInclude(r => r.Replys)
                 .Include(l => l.Lines)
                 .FirstOrDefault();


           
            if (bookc != null)
            {
                var listContent = _context.BookContents.Where(b => b.BookId == idTitulo).ToList();
                int index = listContent.IndexOf(bookc) + 1;
                BookContent? bookCprox;
                if (index <= listContent.Count - 1)
                {
                     bookCprox = listContent[listContent.IndexOf(bookc) + 1];
                }
                else
                {
                    bookCprox = null;
                }
                ContentDTO contentDTO = new() 
                { 
                    BookContentId = bookc.BookContentId,
                    Title = bookc.Title,
                    Book = bookc.Book,
                    AvarageVotes = bookc.AvarageVotes,
                    comments = bookc.comments,
                    Lines = bookc.Lines,
                    BookId = bookc.BookId,
                    ProxName = bookCprox?.Title
                };
                return View(contentDTO);
            }
            return RedirectToAction("Error","Home");
        }
        public async Task<string> PublishComment(CommentDTO comment)
            {
            if (User.Identity.IsAuthenticated)
            {
                if (_context.BookContents.Where(c => c.BookContentId == comment.BookContentId).FirstOrDefault() != null)
                {
                    Comments commentDb = new()
                    {
                        BookContentId = comment.BookContentId,
                        Mensagem = comment.Mensagem,
                        AuthorId = _userManager.GetUserId(User),
                        CommentId = comment.CommentId,
                    };

                    _context.Comments.Add(commentDb);
                    _context.SaveChanges();
                    return "Sucesso";
                }
            }
            
            return "Login";
            

        }

      

        public IActionResult GetAllCaps(int bookId,int contentId)
        {
            var contents = _context.Book.Where(i => i.BookId == bookId).Include(c => c.bookContents).Include(a => a.Author).FirstOrDefault();
            if(contents != null)
            {
                ViewBag.contentId = contentId;
                return PartialView("_CapsPartial",contents);
            }
            else
            {
                return Json(new { error = "Livro inexistente" });
            }
        }

        
        [Route("Pesquisa/{searchString?}")]
        public IActionResult SearchScreen([FromRoute] string? searchString)
        {
            if (searchString == null)
            {
                searchString = "";
            }
            var book = _context.Book
                .Where(b => b.Name.Contains(searchString))
                    .Include(u => u.Author)
                        .Select(l => _mapper.Map<BookDTO>(l)).ToList();

            return View(book);
        }
        [HttpPost]
        public IActionResult SearchItem(int numSearch, string searchString = "")
        {

            switch (numSearch)
            {
                case 1:
                    var book = _context.Book
                        .Where(b => b.Name.Contains(searchString))
                            .Include(u => u.Author)
                                .Select(l => _mapper.Map<BookDTO>(l)).ToList();

                    return PartialView("_BookSearchPartial", book);
                case 2:
                    var user = _context.Users
                        .Where(b => b.UserName.Contains(searchString))
                                .Select(l => _mapper.Map<UserDTO>(l)).ToList();

                    return PartialView("_UserSearchPartial", user);
                case 3:
                    var gender = _context.Genders
                        .Where(b => b.Name.Contains(searchString)).ToList();

                    return PartialView("_GendersSearchPartial", gender);
                default:
                    return RedirectToAction("Error","Home");
            }
        }
    } 
}


       

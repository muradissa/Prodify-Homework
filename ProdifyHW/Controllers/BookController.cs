using Microsoft.AspNetCore.Mvc;
using ProdifyHW.Models;
using System.Threading.Tasks;

namespace ProdifyHW.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Book> books;
            books = _context.Books.ToList();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Book book = new Book();
            return View(book);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Book book = GetBook(Id);
            return View(book);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Book book = GetBook(Id);
            return View(book);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Book book)
        {
            _context.Attach(book);
            _context.Entry(book).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private Book GetBook(int id)
        {
            Book client;
            client = _context.Books
             .Where(c => c.BookID == id).FirstOrDefault();
            return client;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAvailableBook()
        {
            List<Book> books = _context.Books
                .Where(c => c.IsAvailable == true)
                .ToList();
            return Ok(books);
        }

        [HttpGet]
        public async Task<IActionResult> GetTakenClientBooks(int id)
        {
            List<Book> books = _context.Books
                .Where(c => c.ClientID == id)
                .ToList();
            return Ok(books);
        }



        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Book book = GetBook(Id);
            return View(book);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Book book)
        {

            _context.Attach(book);
            _context.Entry(book).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public class ReturnBooksRequest
        {
            public int ClientId { get; set; }
            public int[] ListOfBooksId { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> ReturnBooksToLibrary([FromBody] ReturnBooksRequest request)
        {
            try
            {
                int clientId = request.ClientId;
                int[] listOfBooksId = request.ListOfBooksId;

               
                Client client = _context.Clients.Find(clientId);
                if (client == null)
                {
                    return NotFound("Client not found");
                }

                // Update the books based on the list of book IDs
                foreach (int bookId in listOfBooksId)
                {
                    Book book = _context.Books.Find(bookId);
                    if (book != null && book.ClientID == clientId)
                    {
                        book.ClientID = 0;
                        book.IsAvailable = true;
                    }
                }
                _context.SaveChanges();
                return Ok("Books returned successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error returning books: " + ex.Message);
            }
        }


        [HttpPut]
        public async Task<IActionResult> TakeBooksFromLibrary([FromBody] ReturnBooksRequest request)
        {
            try
            {
                int clientId = request.ClientId;
                int[] listOfBooksId = request.ListOfBooksId;
                Client client = _context.Clients.Find(clientId);
                
                if (client == null)
                {
                    return NotFound("Client not found");
                }
                // Update the books based on the list of book IDs
                foreach (int bookId in listOfBooksId)
                {
                    Book book = _context.Books.Find(bookId);
                    if (book != null && book.ClientID == 0)
                    {
                        book.ClientID = clientId;
                        book.IsAvailable = false;
                        
                    }
                }
                _context.SaveChanges();

                return Ok("Books returned successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error returning books: " + ex.Message);
            }
        }
    }
}

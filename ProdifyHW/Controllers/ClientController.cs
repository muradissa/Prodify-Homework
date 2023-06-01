using Microsoft.AspNetCore.Mvc;
using ProdifyHW.Models;
using System.Diagnostics.Metrics;

namespace ProdifyHW.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Client> clients;
            clients = _context.Clients.ToList();
            return View(clients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Client client = new Client();
            return View(client);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Client client)
        {
            _context.Add(client);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Client client = GetClient(Id);
            return View(client);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Client client = GetClient(Id);
            return View(client);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Client client)
        {
            _context.Attach(client);
            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private Client GetClient(int id)
        {
            Client client;
            client = _context.Clients
             .Where(c => c.ClientID == id).FirstOrDefault();
            return client;

        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Client client = GetClient(Id);
            return View(client);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Client client)
        {

            _context.Attach(client);
            _context.Entry(client).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}

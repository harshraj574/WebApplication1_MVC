using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers.Stored_Procedure
{
    public class StoredController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoredController(ApplicationDbContext context) {
          this._context = context;
        }
        public IActionResult IndexSp()
        {
            var data = _context.Vendors.FromSqlRaw("exec spGetAllVendorList").ToList();
            return View(data);
        }

        public IActionResult Insert_Vendor()
        {
            return View();
        }
    }
}

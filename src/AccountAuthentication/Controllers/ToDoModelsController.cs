using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AccountAuthentication.Data;
using AccountAuthentication.Models.ToDoModels;
using Microsoft.AspNetCore.Authorization;
using AccountAuthentication.Services;
using Microsoft.AspNetCore.Identity;
using AccountAuthentication.Models;
using System.Text;

namespace AccountAuthentication.Controllers
{
    [Authorize]
    public class ToDoModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoModelsController(ApplicationDbContext context, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // GET: ToDoModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoModel.ToListAsync());
        }

        // GET: ToDoModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoModel = await _context.ToDoModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (toDoModel == null)
            {
                return NotFound();
            }

            return View(toDoModel);
        }
        [Authorize(Roles = "Team player,Organizer")]
        // GET: ToDoModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoModels/Create  
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Team player,Organizer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DateTimeStart,DateTimeEnd,Details,Done")] ToDoModel toDoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(toDoModel);
        }
        [Authorize(Roles = "Team player,Organizer")]
        // GET: ToDoModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoModel = await _context.ToDoModel.SingleOrDefaultAsync(m => m.Id == id);
            if (toDoModel == null)
            {
                return NotFound();
            }
            return View(toDoModel);
        }

        // POST: ToDoModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Team player,Organizer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DateTimeStart,DateTimeEnd,Details,Done")] ToDoModel toDoModel)
        {
            if (id != toDoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoModel);
                    await _context.SaveChangesAsync();

                    //String builder to make the message we send
                    var message = new StringBuilder();
                    message.Append("You have completed a task!  ").AppendLine()
                        .Append("The details of your completed message is: ").AppendLine()
                        .Append(toDoModel.Details);

                    if (toDoModel.Done == true)
                    { 
                        //Get user and awaits it, then use the email in the user to send it.
                        var uservar = await _userManager.GetUserAsync(User);
                        await _emailSender.SendEmailAsync(uservar.Email, toDoModel.Title, message.ToString());
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoModelExists(toDoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(toDoModel);
        }
        [Authorize(Roles = "Team Player")]
        // GET: ToDoModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoModel = await _context.ToDoModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (toDoModel == null)
            {
                return NotFound();
            }

            return View(toDoModel);
        }

        // POST: ToDoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Team Player")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoModel = await _context.ToDoModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.ToDoModel.Remove(toDoModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        private bool ToDoModelExists(int id)
        {
            return _context.ToDoModel.Any(e => e.Id == id);
        }
    }
}

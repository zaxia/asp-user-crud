using Microsoft.AspNetCore.Mvc;
using base_test_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace base_test_mvc.Controllers;
public class UsersController : Controller
{

    private readonly Context _context;

    public UsersController(Context context)
    {
        _context = context;
    }

    // GET : Users/List
    public async Task<IActionResult> List()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_Login")))
        {
            return RedirectToAction("Login", "Home");
        }
        var users = _context.User;
        return View(users);
    }
    
    // GET : Users/Add
    public async Task<IActionResult> Add()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_Login")))
        {
            return RedirectToAction("Login", "Home");
        }
        return View();
    }

    // POST: Users/Add
    [HttpPost]
    public async Task<IActionResult> Add(string id, [Bind("login,firstname,lastname,password")] User user)
    {
        try
        {
            user.createdAt = DateTime.Now;
            user.updatedAt = DateTime.Now;
            user.encryptPassword();
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        return RedirectToAction("List");
    }

    // GET : Users/Edit/login
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("_Login")))
        {
            return RedirectToAction("Login", "Home");
        }
        User user = _context.User.Find(id);
        if(user == null) return NotFound();
        return View(user);
    }

    // POST: Users/Edit/login
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Edit(string id, [Bind("login,firstname,lastname")] User userform)
    {
        if (!String.Equals(id, userform.login))
        {
            return NotFound();
        }
        User user = _context.User.Find(id);
        try
        {
            user.updatedAt = DateTime.Now;
            user.firstname=userform.firstname;
            user.lastname=userform.lastname;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        return RedirectToAction("List");
    }

    //AJAX
    // GET : Users/Details/login
    public async Task<IActionResult> Details(string id)
    {
        User user = _context.User.Find(id);
        return Json(user);
    }

    //AJAX
    // POST : Users/Delete/login
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        User user = _context.User.Find(id);
        _context.Remove(user);
        await _context.SaveChangesAsync();
        return Json("ok");
    }

    //AJAX
    // GET : Users/AddRandom
    public async Task<IActionResult> AddRandom()
    {
        User user = Models.User.generateRandom();
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(List));
    }

    //AJAX
    // POST : Users/CheckUsername
    [HttpPost]
    public async Task<IActionResult> CheckUsername(string login)
    {
        User user = _context.User.Find(login);
        if (user == null)
        {
            return Json("unique");
        }
        return Json("not unique");
    }
}

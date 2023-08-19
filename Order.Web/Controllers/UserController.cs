using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.DataAccess.DbContext;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;

namespace Order.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, OrderDbContext db)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetAll().ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit(string Id)
        {
            if(Id == null)
            {
                  return NotFound();
            }
            var user = _unitOfWork.User.Get(i => i.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
              return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = _unitOfWork.User.Get(i => i.Id == user.Id);

                var role = _roleManager.FindByNameAsync(user.Role).Result;
                var userRole = _userManager.GetRolesAsync(userToUpdate).Result;
                if (userToUpdate == null)
                {
                    return NotFound();
                }
                userToUpdate.Name = user.Name;
                userToUpdate.EmailConfirmed = user.EmailConfirmed;
                userToUpdate.Role = user.Role;
                _userManager.AddToRoleAsync(userToUpdate, role.Name).Wait();
                _unitOfWork.User.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var user = _unitOfWork.User.Get(i => i.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteUser(string Id)
        {
            var loggedInUser = _userManager.GetUserId(HttpContext.User);
            if(loggedInUser == Id)
            {
                return BadRequest("You cannot delete the currently logged-in user.");
            }
            var user = _unitOfWork.User.Get(i => i.Id == Id);
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.User.Delete(user);
            _unitOfWork.User.Save();
            return RedirectToAction("Index");
        }
    }
}

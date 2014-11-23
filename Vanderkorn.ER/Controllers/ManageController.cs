﻿namespace Vanderkorn.ER.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Security;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using Vanderkorn.ER.Models;

    [System.Web.Mvc.Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ManageController()
        {
            this.applicationDbContext = new ApplicationDbContext();
        }

        public ManageController(ApplicationUserManager userManager)
        {
            this.applicationDbContext = new ApplicationDbContext();
            this.UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this._userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index()
        {
            var receptions = this.applicationDbContext.Receptions.ToList();

           var user =  await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            var receptionId = user.ReceptionId;

            var model = new IndexViewModel
            {
                ReceptionId = receptionId,
                Receptions = receptions
            };
            return View(model);
        }


        [System.Web.Mvc.HttpGet]
        public async Task<bool> BecomeMinister()
        {
            var userId = this.User.Identity.GetUserId();
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user == null)
            {
                return false;
            }

            var res = this.applicationDbContext.Receptions.FirstOrDefault(s => s.MinisterId == userId);
            if (res == null)
            {
                await this.UserManager.AddToRoleAsync(userId, "Minister");
                this.applicationDbContext.Receptions.Add(new Reception()
                                                             {
                                                                 Name = user.UserName,
                                                                 MinisterId = userId,
                                                                 ModifyDate = DateTime.UtcNow
                                                             }
                    );
                await this.applicationDbContext.SaveChangesAsync();
            }
            return true;
        }
        [System.Web.Mvc.HttpPost]
        public async Task<bool> BecomeSecretary([FromBody]long ReceptionId)
        {
            var userId = this.User.Identity.GetUserId();
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());

            //  || await UserManager.IsInRoleAsync(userId, "Minister")
            if (user == null)
            {
                return false;
            }
            var res = await this.applicationDbContext.Receptions.FindAsync(ReceptionId);
            if (res == null)
            {
                return false;
            }

            await this.UserManager.AddToRoleAsync(userId, "Secretary");

            user.ReceptionId = ReceptionId;

            await this.UserManager.UpdateAsync(user);

            return true;
        }
    }
}
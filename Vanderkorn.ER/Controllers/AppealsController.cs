namespace Vanderkorn.ER.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin;

    using Vanderkorn.ER.Models;

    public static class  HttpExtensions
{
      public static IOwinContext GetOwinContext(this HttpRequestMessage request)
        {
            var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
            if (context != null)
            {
                return HttpContextBaseExtensions.GetOwinContext(context.Request);
            }
            return null;
        }

}
    [System.Web.Http.Authorize(Roles = "Admin, Minister, Secretary")]
    public class AppealsController : ApiController
    {
      
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext applicationDbContext;
        public AppealsController()
        {
            this.applicationDbContext = new ApplicationDbContext();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this._userManager = value;
            }
        }

        /// <summary>
        /// Получить обращения
        /// </summary>
        /// <returns></returns>
        public IList<Appeal> Get()
        {
           var userId =  this.User.Identity.GetUserId();
           var user = this.UserManager.FindById(userId);

           var receptionId = user.ReceptionId;

           if (receptionId ==null || receptionId <= 0)
            {
                var reception = this.applicationDbContext.Receptions.FirstOrDefault(r => r.MinisterId == user.Id);
                if (reception != null)
                {
                    receptionId = reception.Id;
                }
            }


           var appeals = this.applicationDbContext.Appeals.Where(r => r.ReceptionId == receptionId).ToList();
           return appeals;
        }

        // POST api/values
        [ResponseType(typeof(Appeal))]
        [System.Web.Http.Authorize(Roles = "Admin, Secretary")]
        public IHttpActionResult Post([FromBody]Appeal value)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.UserManager.FindById(userId);
            var receptionId = user.ReceptionId;
            if (receptionId == null && receptionId <= 0)
            {
                return this.BadRequest();
            }
            value.IsExecuted = false;
            value.Comment = string.Empty;
            value.DecisionType = DecisionType.None;
            value.ModifyDate = DateTime.UtcNow;
            value.ReceptionId = receptionId.Value;

            var appeal = this.applicationDbContext.Appeals.Add(value);
            this.applicationDbContext.SaveChanges();

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ReceptionHub>();
         
            var connections = ReceptionHub._connections.GetConnections(this.User.Identity.Name);

            hubContext.Clients.Group(receptionId.Value.ToString(CultureInfo.InvariantCulture), connections.ToArray()).updateAppeal(appeal);

            return this.Ok(appeal);
        }

        // PUT api/values/5
        [HttpPut]
        [System.Web.Http.Authorize(Roles = "Admin, Minister")]
        public IHttpActionResult Decide(int id, Appeal value)
        {
            var comment = value.Comment;
            var desicion = value.DecisionType;
            var userId = this.User.Identity.GetUserId();
            var user = this.UserManager.FindById(userId);
            var reception = this.applicationDbContext.Receptions.FirstOrDefault(r => r.MinisterId == user.Id);
            if (reception == null)
            {
                return this.BadRequest();
            }

            var receptionId = reception.Id;

            var appeal = this.applicationDbContext.Appeals.FirstOrDefault(r => r.Id == id && r.ReceptionId == receptionId);
            if (appeal == null)
            {
                return this.BadRequest();
            }

            if (appeal.IsExecuted)
            {
                return this.BadRequest();
            }

            if (appeal != null)
            {
                appeal.Comment = comment;
                appeal.DecisionType = desicion;
                this.applicationDbContext.Appeals.AddOrUpdate(appeal);
                this.applicationDbContext.SaveChanges();

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ReceptionHub>();

                var connections = ReceptionHub._connections.GetConnections(this.User.Identity.Name);

                hubContext.Clients.Group(receptionId.ToString(CultureInfo.InvariantCulture), connections.ToArray()).updateAppeal(appeal);

                return this.Ok();
            }
            return this.BadRequest();
        }

        [HttpPut]
        [System.Web.Http.Authorize(Roles = "Admin, Secretary")]
        public IHttpActionResult Execute(int id, [FromBody]Appeal value)
        {
            var isExecuted  = value.IsExecuted;
            var userId = this.User.Identity.GetUserId();
            var user = this.UserManager.FindById(userId);
            var receptionId = user.ReceptionId;
            if (receptionId == null || receptionId <= 0)
            {
                return this.BadRequest();
            }

            var appeal = this.applicationDbContext.Appeals.FirstOrDefault(r => r.Id == id && r.ReceptionId == receptionId);
            if (appeal == null)
            {
                return this.BadRequest();
            }

            if (appeal.DecisionType == DecisionType.None)
            {
                return this.BadRequest();
            }

            if (appeal != null)
            {
                appeal.IsExecuted = isExecuted;
                this.applicationDbContext.Appeals.AddOrUpdate(appeal);
                this.applicationDbContext.SaveChanges();

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ReceptionHub>();

                var connections = ReceptionHub._connections.GetConnections(this.User.Identity.Name);

                hubContext.Clients.Group(receptionId.Value.ToString(CultureInfo.InvariantCulture), connections.ToArray()).updateAppeal(appeal);

                return this.Ok();
            }

            return this.BadRequest();
        }

    }
}

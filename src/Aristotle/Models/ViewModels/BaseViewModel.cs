using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Claims;
using Aristotle.Models;
using Aristotle.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Aristotle.ViewModels
{
    public class BaseViewModel
    {
        protected ApplicationDbContext context;

        public BaseViewModel(ApplicationDbContext ctx, ApplicationUser user)
        {
            context = ctx;
            _user = user;

        }

        public BaseViewModel() { }

        private ApplicationUser _user;

        public string getLoggedInUserId()
        {
            return _user.Id;
        }
    }
}

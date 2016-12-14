using System.Collections.Generic;
using System.Linq;
using Aristotle.Models;
using Aristotle.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Aristotle.ViewModels
{

    public class AddClassView : BaseViewModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Title { get; set; }
        public string Subject { get; set; }

        public AddClassView(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) { }
        public AddClassView() { }
    }
}
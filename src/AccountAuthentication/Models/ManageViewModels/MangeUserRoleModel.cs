using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAuthentication.Models.ManageViewModels
{
    public class ManageUserRoleModel
    {
        public ApplicationUser User { get; set; }
        public SelectList Roles { get; set; }

    }
}

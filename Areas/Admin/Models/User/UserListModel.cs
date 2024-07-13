using System.Collections.Generic;
using AspMVC.Models;
using X.PagedList;

namespace AspMVC.Areas.Identity.Models.UserViewModels
{
        public class UserListModel
        {
            public int totalUsers { get; set; }
            //public int countPages { get; set; }

            //public int ITEMS_PER_PAGE { get; set; } = 15;

            //public int currentPage { get; set; }

            public IPagedList<UserAndRole> users { get; set; }

        }

        public class UserAndRole : AppUser
        {
            public string RoleNames { get; set; }
        }


}
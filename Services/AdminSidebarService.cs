using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AspMVC.AdminMenu
{
    public class AdminSidebarService
    {
        private readonly IUrlHelper UrlHelper;
        public List<AdminSidebarItem> Items { get; set; } = new List<AdminSidebarItem>();


        public AdminSidebarService(IUrlHelperFactory factory, IActionContextAccessor action)
        {
            UrlHelper = factory.GetUrlHelper(action.ActionContext);
            // Khoi tao cac muc sidebar
            Items.Add(new AdminSidebarItem() { Type = AdminSidebarItemType.Heading, Title = "Quản lý chung" });

            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Controller = "DbManage",
                Action = "Index",
                Area = "Database",
                Title = "Quản lý Database",
                AwesomeIcon = "fas fa-database"
            });

            Items.Add(new AdminSidebarItem() { Type = AdminSidebarItemType.Divider });

            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Title = "Quản lý thành viên",
                AwesomeIcon = "far fa-folder",
                collapseID = "member",
                Items = new List<AdminSidebarItem>() {
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "User",
                                Action = "Index",
                                Area = "Admin",
                                Title = "Danh sách thành viên"
                        },
                    },
            });
            Items.Add(new AdminSidebarItem() { Type = AdminSidebarItemType.Divider });

            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Title = "Quản lý vai trò",
                AwesomeIcon = "far fa-folder",
                collapseID = "role",
                Items = new List<AdminSidebarItem>() {
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Index",
                                Area = "Admin",
                                Title = "Danh sách vai trò"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Create",
                                Area = "Admin",
                                Title = "Tạo vai trò mới"
                        },
                    },
            });
            Items.Add(new AdminSidebarItem() { Type = AdminSidebarItemType.Divider });

            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Title = "Quản lý thể loại",
                AwesomeIcon = "far fa-folder",
                collapseID = "blog",
                Items = new List<AdminSidebarItem>() {
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Genre",
                                Action = "Index",
                                Area = "Admin",
                                Title = "Các Genre"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Tag",
                                Action = "Index",
                                Area = "Admin",
                                Title = "Các Tag"
                        },

                    },
            });




        }


        public string renderHtml()
        {
            var html = new StringBuilder();

            foreach (var item in Items)
            {
                html.Append(item.HtmlContent(UrlHelper));
            }


            return html.ToString();
        }



    }
}

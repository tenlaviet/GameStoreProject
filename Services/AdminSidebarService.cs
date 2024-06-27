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
            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Controller = "Contact",
                Action = "Index",
                Area = "Contact",
                Title = "Quản lý liên hệ",
                AwesomeIcon = "far fa-address-card"
            });
            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Title = "Phân quyền & thành viên",
                AwesomeIcon = "far fa-folder",
                collapseID = "role",
                Items = new List<AdminSidebarItem>() {
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Index",
                                Area = "Admin",
                                Title = "Các vai trò (role)"
                        },
                         new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Create",
                                Area = "Admin",
                                Title = "Tạo role mới"
                        },
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
                Title = "Quản lý Game",
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
                                Controller = "Category",
                                Action = "Create",
                                Area = "Blog",
                                Title = "Tạo chuyên mục"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Index",
                                Area = "Blog",
                                Title = "Các bài viết"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Create",
                                Area = "Blog",
                                Title = "Tạo bài viết"
                        },
                    },
            });
            Items.Add(new AdminSidebarItem() { Type = AdminSidebarItemType.Divider });
            Items.Add(new AdminSidebarItem()
            {
                Type = AdminSidebarItemType.NavItem,
                Title = "Quản lý sản phẩm",
                AwesomeIcon = "far fa-folder",
                collapseID = "product",
                Items = new List<AdminSidebarItem>() {
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "CategoryProduct",
                                Action = "Index",
                                Area = "Product",
                                Title = "Các chuyên mục"
                        },
                         new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "CategoryProduct",
                                Action = "Create",
                                Area = "Product",
                                Title = "Tạo chuyên mục"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "ProductManage",
                                Action = "Index",
                                Area = "Product",
                                Title = "Các sản phẩm"
                        },
                        new AdminSidebarItem() {
                                Type = AdminSidebarItemType.NavItem,
                                Controller = "ProductManage",
                                Action = "Create",
                                Area = "Product",
                                Title = "Tạo sản phẩm"
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

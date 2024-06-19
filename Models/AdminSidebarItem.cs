using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AspMVC.AdminMenu
{
    public enum AdminSidebarItemType
    {
        Divider,
        Heading,
        NavItem
    }
    public class AdminSidebarItem
    {
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public AdminSidebarItemType Type { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Area { get; set; }

        public string AwesomeIcon { get; set; } // fas fa-fw fa-cog


        public List<AdminSidebarItem> Items { get; set; }

        public string collapseID { get; set; }

        public string GetLink(IUrlHelper urlHelper)
        {
            return urlHelper.Action(Action, Controller, new { area = Area });
        }
        public string HtmlContent(IUrlHelper urlHelper)
        {
            var html = new StringBuilder();


            if (Type == AdminSidebarItemType.Heading)
            {
                html.Append(@$"<div class=""sb-sidenav-menu-heading"">{Title}</div>");
            }
            else if (Type == AdminSidebarItemType.NavItem)
            {
                if (Items == null)
                {
                    var url = GetLink(urlHelper);
                    var icon = (AwesomeIcon != null) ?
                                $"<i class=\"{AwesomeIcon}\"></i>" :
                                "";

                    var cssClass = "nav-link";
                    if (IsActive) cssClass += " active";


                    html.Append(@$"
                        <a class=""{cssClass}"" href=""{url}"">
                            <div class=""sb-nav-link-icon"">
                            {icon}
                            </div>
                            {Title}
                        </a>                   
                    ");

                }
                else // Items != null
                {
                    
                    var cssClass = "nav-item";

                    var icon = (AwesomeIcon != null) ?
                                $"<i class=\"{AwesomeIcon}\"></i>" : "";


                    var itemMenu = "";

                    foreach (var item in Items)
                    {
                        var urlItem = item.GetLink(urlHelper);
                        var cssItem = "nav-link";
                        itemMenu += $"<a class=\"{cssItem}\" href=\"{urlItem}\">{item.Title}</a>";
                    }

                    html.Append(@$"
                    
                        <a class=""nav-link collapsed"" href=""#"" data-bs-toggle=""collapse"" data-bs-target=""#{collapseID}s"" aria-expanded=""false"" aria-controls=""{collapseID}s"">
                            <div class=""sb-nav-link-icon"">{icon}</div>
                            {Title}
                            <div class=""sb-sidenav-collapse-arrow""><i class=""fas fa-angle-down""></i></div>
                        </a>
                        <div class=""collapse"" id=""{collapseID}s"" aria-labelledby=""headingOne"" data-bs-parent=""#sidenavAccordion"">
                            <nav class=""sb-sidenav-menu-nested nav"">
                                {itemMenu}
                            </nav>
                        </div>                  
                    
                    ");


                }
            }

            return html.ToString();
        }
    }
}

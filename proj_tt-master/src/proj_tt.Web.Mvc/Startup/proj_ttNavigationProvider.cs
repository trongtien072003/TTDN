using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using proj_tt.Authorization;

namespace proj_tt.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class proj_ttNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.About,
                        L("About"),
                        url: "About",
                        icon: "fas fa-info-circle"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Tasks,
                        L("Tasks"),
                        url: "Tasks",
                        icon: "fas fa-tasks"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Orders,
                        L("Orders"),
                        url: "admin/orders",
                        icon: "fas fa-clipboard-list",
                        requiresAuthentication: true,
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Orders)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Products,
                        L("Product"),
                        url: "Product",
                        icon: "fas fa-box"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        "Categories",
                        L("Categories"),
                        url: "Categories",
                        icon: "fas fa-box"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        "View",
                        L("ViewUser"),
                        url: "https://localhost:44312/",
                        icon: "fas fa-box",
                        target: "_blank"
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Tenants,
                        L("Tenants"),
                        url: "Tenants",
                        icon: "fas fa-building",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, proj_ttConsts.LocalizationSourceName);
        }
    }
}

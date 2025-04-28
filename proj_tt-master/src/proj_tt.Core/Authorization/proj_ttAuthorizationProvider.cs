﻿using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace proj_tt.Authorization
{
    public class proj_ttAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var productPermission = context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Create, L("CreateProduct"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Edit, L("EditProduct"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Delete, L("DeleteProduct"));

            var ordersPermission = context.CreatePermission(PermissionNames.Pages_Orders, L("Orders"));
            ordersPermission.CreateChildPermission(PermissionNames.Pages_Orders_Create, L("CreateNewOrder"));
            ordersPermission.CreateChildPermission(PermissionNames.Pages_Orders_Edit, L("EditOrder"));
            ordersPermission.CreateChildPermission(PermissionNames.Pages_Orders_Delete, L("DeleteOrder"));
            ordersPermission.CreateChildPermission(PermissionNames.Pages_Orders_View, L("ViewOrder"));

            // Cart permissions
            var cartPermission = context.CreatePermission(PermissionNames.Pages_Cart, L("Cart"));
            cartPermission.CreateChildPermission(PermissionNames.Pages_Cart_View, L("ViewCart"));
            cartPermission.CreateChildPermission(PermissionNames.Pages_Cart_Add, L("AddToCart"));
            cartPermission.CreateChildPermission(PermissionNames.Pages_Cart_Update, L("UpdateCart"));
            cartPermission.CreateChildPermission(PermissionNames.Pages_Cart_Remove, L("RemoveFromCart"));
            cartPermission.CreateChildPermission(PermissionNames.Pages_Cart_Checkout, L("Checkout"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, proj_ttConsts.LocalizationSourceName);
        }
    }
}

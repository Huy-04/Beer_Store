namespace BeerStore.Domain.Constants.Permission
{
    public static class ShopConstant
    {
        public static class Store
        {
            // Read
            public const string ReadAll = "Store.Read.All";
            public const string ReadSelf = "Store.Read.Self";
            
            // Write
            public const string CreateAll = "Store.Create.All";
            public const string CreateSelf = "Store.Create.Self";
            public const string UpdateAll = "Store.Update.All";
            public const string UpdateSelf = "Store.Update.Self";
            public const string DeleteAll = "Store.Delete.All";
            public const string DeleteSelf = "Store.Delete.Self";

            // Admin
            public const string ApproveAll = "Store.Approve.All";
            public const string RejectAll = "Store.Reject.All";
            public const string SuspendAll = "Store.Suspend.All";
            public const string BanAll = "Store.Ban.All";
            public const string ReactivateAll = "Store.Reactivate.All";
        }

        public static class StoreAddress
        {
            public const string ReadAll = "StoreAddress.Read.All";
            public const string ReadSelf = "StoreAddress.Read.Self";
            public const string CreateAll = "StoreAddress.Create.All";
            public const string CreateSelf = "StoreAddress.Create.Self";
            public const string UpdateAll = "StoreAddress.Update.All";
            public const string UpdateSelf = "StoreAddress.Update.Self";
            public const string DeleteAll = "StoreAddress.Delete.All";
            public const string DeleteSelf = "StoreAddress.Delete.Self";
            public const string ManageAll = "StoreAddress.Manage.All";
            public const string ManageSelf = "StoreAddress.Manage.Self";
        }
    }
}

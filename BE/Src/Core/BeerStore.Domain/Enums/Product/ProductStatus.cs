namespace BeerStore.Domain.Enums.Product
{
    public enum ProductStatus
    {
        Draft = 0,      // Đang soạn, chưa publish
        Active = 1,     // Đang bán
        Inactive = 2,   // Tạm ẩn (seller tự ẩn)
        OutOfStock = 3, // Hết hàng (auto từ Inventory)
        Deleted = 4     // Soft delete
    }
}

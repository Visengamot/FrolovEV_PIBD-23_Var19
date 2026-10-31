
namespace ShoeStore.WinForms.Entities;

public sealed record StockRow(Guid ShoeId, string Article, string ShoeName, string ManufacturerName, Guid ShoeSizeId, decimal Size, int Balance, int CurrentMonthSold, decimal CurrentMonthRevenue);

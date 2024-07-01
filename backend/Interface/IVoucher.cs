using backend.Dto;
using backend.Dto.Voucher;

namespace backend.Interface;

public interface IVoucher
{
    Task<IEnumerable<VoucherItem>> GetAll();
    Task<VoucherDto?> GetOne(int id);
    Task<VoucherDto?> Create(VoucherCreate voucherCreate);
    Task<VoucherDto?> Update(VoucherUpdate voucherUpdate, int id);
    Task<ApiObject?> DeleteOne(int id);
    Task<ApiObject?> AddToVoucher(AddToVoucher addToVoucher);
    Task<IEnumerable<VoucherItemOfCustomer>> MyListVoucher();
    Task<IEnumerable<VoucherItem>?> MyVoucherOfStore(int storeId);
}
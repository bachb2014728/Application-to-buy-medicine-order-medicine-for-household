using backend.Dto;
using backend.Dto.Auth;
using backend.Dto.Customer;

namespace backend.Interface;

public interface ICustomer
{
    Task<IEnumerable<CustomerItem>?> GetAll();
    Task<CustomerFullview?> GetOne(int id);
    Task<CustomerFullview?> GetProfile();
    Task<CustomerFullview?> Update(CustomerUpdate customerUpdate, int id);
}
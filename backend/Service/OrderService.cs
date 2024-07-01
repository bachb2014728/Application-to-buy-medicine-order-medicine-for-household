using backend.Data;
using backend.Dto;
using backend.Dto.Order;
using backend.Dto.Order.OrderDetail;
using backend.Dto.Voucher;
using backend.Error;
using backend.Helper;
using backend.Interface;
using backend.Model;
using Microsoft.EntityFrameworkCore;

namespace backend.Service;

public class OrderService(ApplicationDbContext context, ConvertInformation convert) : IOrder
{
    private readonly ApplicationDbContext _context = context;
    private readonly ConvertInformation _convert = convert;
    public async Task<IEnumerable<OrderItem>> GetAll()
    {
        var orders = await _context.Orders.ToListAsync();
        if (orders.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn nào.");
        }

        return orders.Select(ToOrderItem);
    }

    public async Task<IEnumerable<OrderItem>> MyListOrder()
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var orders = await _context.Orders
            .Include(x=>x.OrderDetails)
            .Include(x=>x.Vouchers)
            .Where(or=>or.CustomerId == customer.Id).ToListAsync();
        if (orders.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn nào.");
        }
        return orders.Select(ToOrderItem);
    }

    public async Task<OrderDto?> GetOne(int id)
    {
        var order = await CheckOrder(id);
        return ConvertOrderDto(order);
    }

    public async Task<OrderDto?> Create(OrderCreate orderCreate)
    {
        var store = await _context.Stores
            .Include(store => store.Orders)
            .FirstOrDefaultAsync(x => x.Id == orderCreate.StoreId);
        if (store == null)
        {
            throw new NotFoundException($"không tìm thấy cửa hàng cần tìm với id = {orderCreate.StoreId}");
        }
        var order = new Order
        {
            TotalPrice = orderCreate.TotalPrice,
            ReceiverName = orderCreate.ReceiverName,
            ReceiverPhone = orderCreate.ReceiverPhone,
            ReceiverAddress = orderCreate.ReceiverAddress,
            OrderDetails = await ConvertCartToOrderDetail(orderCreate.Carts),
            CreatedOn = DateTime.UtcNow.AddHours(7),
            Payment = orderCreate.Payment,
            OrderStatus = StatusData.processing,
            Customer = await _convert.ToCustomerFormUser(GlobalVariables.Token)
        };
        await _context.Orders.AddAsync(order);
        if (orderCreate.Vouchers != null)
        {
            var listVouchers = await _context.CustomerVouchers
                .Where(x => orderCreate.Vouchers.Contains(x.VoucherId)).ToListAsync();
            foreach (var item in listVouchers)
            {
                item.IsUsed = true;
                await _context.SaveChangesAsync();
            }

            var vouchers = await _context.Vouchers
                .Where(x => orderCreate.Vouchers.Contains(x.Id)).ToListAsync();
            order.Vouchers = vouchers;
        }
        await _context.SaveChangesAsync();
        store.Orders.Add(order);
        await _context.SaveChangesAsync();
        return ConvertOrderDto(order);
    }

    public async Task<OrderDto?> Update(OrderUpdate orderUpdate,int id)
    {
        var customer = await _convert.ToCustomerFormUser(GlobalVariables.Token);
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        if (order == null)
        {
            throw new NotFoundException("Không tìm thấy dữ liệu");
        }

        if (customer.Id != order.CustomerId)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn trong tài khoản hiện tại.");
        }

        order.ReceiverName = orderUpdate.ReceiverName ?? throw new InvalidFormatException("Dữ liệu nhập bị trống.");
        order.ReceiverPhone = orderUpdate.ReceiverPhone ?? throw new InvalidFormatException("Dữ liệu nhập bị trống.");
        order.ReceiverAddress =
            orderUpdate.ReceiverAddress ?? throw new InvalidFormatException("Dữ liệu nhập bị trống.");
        var notification = new Notification
        {
            CustomerId = order.CustomerId,
            StoreId = order.StoreId,
            Message = $"Đơn hàng {order.TotalPrice} được cập nhật",
            IsRead = false,
            CreatedOn = DateTime.UtcNow.AddHours(7)
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return ConvertOrderDto(order);
    }

    public async Task<IEnumerable<OrderItem>?> GetAllOrderOfStore(int storeId)
    {
        var orders = await _context.Orders
            .Include(x=>x.OrderDetails)
            .Include(x=>x.Vouchers)
            .Where(or=>or.StoreId == storeId).ToListAsync();
        if (orders.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn nào.");
        }
        return orders.Select(ToOrderItem);
    }

    public async Task<object?> ChangeStatusOrder(ChangeStatus changeStatus, int orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        if (order == null)
        {
            throw new NotFoundException("Không tim thấy cửa hàng.");
        }
        switch (changeStatus.OrderStatus)
        {
            case StatusData.shipping when order.OrderStatus == StatusData.processing:
                order.OrderStatus = StatusData.shipping;
                var notification = new Notification
                {
                    CustomerId = order.CustomerId,
                    StoreId = order.StoreId,
                    Message = $"Đơn hàng {order.TotalPrice} đang vận chuyển",
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow.AddHours(7)
                };
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                break;
            case StatusData.delivered when order.OrderStatus == StatusData.shipping:
                order.OrderStatus = StatusData.delivered;
                var notification2 = new Notification
                {
                    CustomerId = order.CustomerId,
                    StoreId = order.StoreId,
                    Message = $"Đơn hàng {order.TotalPrice} thành công",
                    IsRead = false,
                    CreatedOn = DateTime.UtcNow.AddHours(7)
                };
                await _context.Notifications.AddAsync(notification2);
                await _context.SaveChangesAsync();
                break;
            case StatusData.processing:
                break;
            case StatusData.canceled:
                break;
            default:
                throw new InvalidFormatException("Lỗi khi thay đổi trạng thái không đúng.");
        }
        return new ApiObject() { Message = "Thay đổi thành công." };
    }

    public async Task<object?> OrderCancel(int orderId)
    {
        var order = await _context.Orders
            .Include(x=>x.Vouchers)
            .Include(x=>x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == orderId);
        if (order == null)
        {
            throw new NotFoundException("Không tim thấy cửa hàng.");
        }

        if (order.OrderStatus != StatusData.processing)
        {
            throw new InvalidFormatException("Không thể hủy đơn hàng khi trạng thái đơn hàng đã hàng thành.");
        }
        order.OrderStatus = StatusData.canceled;
        if (order.OrderDetails != null)
        {
            var products = await _context.Products
                .Where(product => order.OrderDetails.Select(item=>item.ProductId).Contains(product.Id))
                .ToListAsync();
            foreach (var product in products)
            {
                var orderItem = await _context.OrderDetails
                    .FirstOrDefaultAsync(x => x.ProductId == product.Id);
                if (orderItem != null)
                {
                    product.Quantity += orderItem.TotalQuantity;
                }

                await _context.SaveChangesAsync();
            }
        }

        if (order.Vouchers != null)
        {
            var vouchers = await _context.CustomerVouchers
                .Where(x=> order.Vouchers.Select(item=>item.Id).Contains(x.VoucherId))
                .ToListAsync();
            foreach (var cusVoucher in vouchers)
            {
                var voucher = await _context.Vouchers.Include(voucher => voucher.Orders).FirstOrDefaultAsync(x => x.Id == cusVoucher.VoucherId);
                if (voucher.Orders == null) continue;
                voucher.Orders.Remove(order);
                cusVoucher.IsUsed = false;
                await _context.SaveChangesAsync();

            }
        }
        var notification = new Notification
        {
            CustomerId = order.CustomerId,
            StoreId = order.StoreId,
            Message = $"Đơn hàng {order.TotalPrice} đã hủy",
            IsRead = false,
            CreatedOn = DateTime.UtcNow.AddHours(7)
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return new ApiObject() { Message = "Thay đổi thành công." };
    }

    public async Task<object?> DeleteOne(int id)
    {
        var order = await _context.Orders
            .Include(x=>x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (order == null)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn.");
        }

        if (order.OrderDetails != null)
        {
            var orderItems = await _context.OrderDetails
                .Where(x => order.OrderDetails.Select(or => or.Id).Contains(x.Id))
                .ToListAsync();
            foreach (var item in orderItems)
            {
                _context.OrderDetails.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return new ApiObject() { Message = "Xóa hóa đơn thành công." };
    }

    private async Task<List<OrderDetail>> ConvertCartToOrderDetail(List<int> carts)
    {
        List<OrderDetail> orderDetails = [];
        var listCart = await _context.Carts
            .Include(x=>x.Product)
            .Where(x => carts.Contains(x.Id)).ToListAsync();
        
        foreach (var item in listCart)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
            if (product==null)
            {
                throw new NotFoundException("Không tìm thấy sản phẩm");
            }
            var priceNow = product.Sale == 0 ? product.Price : product.Sale;
            var orderDetail = new OrderDetail
            {
                ProductId = product.Id,
                ProductName = item.Product?.Name,
                PriceNow = priceNow ,
                TotalQuantity = item.Quantity,
                TotalPrice = item.Quantity * priceNow
            };
            if (item.Quantity > product.Quantity)
            {
                throw new InvalidFormatException("Số lượng không đủ để thực hiện chức năng.");
            }
            product.Quantity -= item.Quantity;
            await _context.OrderDetails.AddAsync(orderDetail);
            _context.Carts.Remove(item);
            await _context.SaveChangesAsync();
            orderDetails.Add(orderDetail);
        }

        return orderDetails;
    }

    private async Task<Order> CheckOrder(int id)
    {
        var order = await _context.Orders.Include(or=>or.OrderDetails).FirstOrDefaultAsync(or => or.Id == id);
        if (order == null)
        {
            throw new NotFoundException($"Không tìm thấy dữ liệu với id = {id}");
        }

        return order;
    }
    private static OrderItem ToOrderItem(Order order)
    {
        var orderItem =  new OrderItem
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            ReceiverName = order.ReceiverName,
            ReceiverPhone = order.ReceiverPhone,
            ReceiverAddress = order.ReceiverAddress,
            Products = order.OrderDetails?.Select(ToOrderDetailItem).ToList(),
            CreatedOn = order.CreatedOn,
            Payment = order.Payment,
            OrderStatus = order.OrderStatus,
            CustomerId = order.CustomerId
        };
        if (order.Vouchers !=null)
        {
            orderItem.Vouchers = order.Vouchers.Select(ToVoucherItem);
        }

        return orderItem;
    }
    private static VoucherItem ToVoucherItem(Voucher voucher)
    {
        
        return new VoucherItem
        {
            Id = voucher.Id,
            Title = voucher.Title,
            Code = voucher.Code,
            DiscountAmount = voucher.DiscountAmount,
            DiscountPercentage = voucher.DiscountPercentage,
            StartTime = voucher.StartTime,
            EndTime = voucher.EndTime,
            IsGlobal = voucher.StoreId == null ? true : false,
            StoreId = voucher.StoreId
        };
    }

    private static OrderDetailItem ToOrderDetailItem(OrderDetail orderDetail)
    {
        return new OrderDetailItem
        {
            Id = orderDetail.Id,
            ProductId = orderDetail.ProductId,
            ProductName = orderDetail.ProductName,
            Price = orderDetail.PriceNow,
            TotalQuantity = orderDetail.TotalQuantity,
            TotalPrice = orderDetail.TotalPrice
        };
    }

    private static OrderDto ConvertOrderDto(Order order)
    {
        List<VoucherItem> listVoucherItems = [];
        if (order.Vouchers != null)
        {
            listVoucherItems.AddRange(order.Vouchers.Select(item => new VoucherItem
            {
                Id = item.Id,
                Title = item.Title,
                Code = item.Code,
                DiscountAmount = item.DiscountAmount,
                DiscountPercentage = item.DiscountPercentage,
                StartTime = item.StartTime,
                EndTime = item.EndTime
            }));
        }
        List<OrderDetailItem> listItem = [];
        if (order.OrderDetails != null)
            listItem.AddRange(order.OrderDetails.Select(item => new OrderDetailItem
            {
                Id = item.Id,
                ProductName = item.ProductName,
                Price = item.PriceNow,
                TotalQuantity = item.TotalQuantity,
                TotalPrice = item.TotalPrice
            }));
        return new OrderDto
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            ReceiverName = order.ReceiverName,
            ReceiverPhone = order.ReceiverPhone,
            ReceiverAddress = order.ReceiverAddress,
            OrderDetails = listItem,
            Payment = order.Payment,
            Vouchers = listVoucherItems,
            CreatedOn = order.CreatedOn,
            OrderStatus = order.OrderStatus,
            CustomerId = order.CustomerId
        };
    }
}
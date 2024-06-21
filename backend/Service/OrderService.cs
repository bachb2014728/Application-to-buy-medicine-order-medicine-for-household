using backend.Data;
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
        var orders = await _context.Orders.Where(or=>or.CustomerId == customer.Id).ToListAsync();
        if (orders.Count == 0)
        {
            throw new NotFoundException("Không tìm thấy hóa đơn nào.");
        }
        return orders.Select(ToOrderItem);
    }

    public async Task<OrderDto?> GetOne(int id)
    {
        var order = await CheckOrder(id);
        return await ConvertOrderDto(order);
    }

    public async Task<OrderDto?> Create(OrderCreate orderCreate)
    {
        var order = new Order
        {
            TotalPrice = orderCreate.TotalPrice,
            ReceiverName = orderCreate.ReceiverName,
            ReceiverPhone = orderCreate.ReceiverPhone,
            ReceiverAddress = orderCreate.ReceiverAddress,
            OrderDetails = await ConvertCartToOrderDetail(orderCreate.CartIds),
            CreatedOn = DateTime.UtcNow.AddHours(7),
            OrderStatus = "Chuyển bị đơn hàng.",
            Customer = await _convert.ToCustomerFormUser(GlobalVariables.Token)
        };
        await _context.Orders.AddAsync(order);
        var listVouchers = await _context.CustomerVouchers
            .Where(x => orderCreate.VoucherIds.Contains(x.Id)).ToListAsync();
        foreach (var item in listVouchers)
        {
            var orderVoucher = new OrderVouchers
            {
                Order = order,
                Voucher = await _context.Vouchers.FirstOrDefaultAsync(x => x.Id == item.VoucherId)
            };
            await _context.OrderVouchers.AddAsync(orderVoucher);
            item.IsUsed = true;
            await _context.SaveChangesAsync();
        }

        return await ConvertOrderDto(order);
    }

    public Task<OrderDto?> Update(OrderUpdate orderUpdate,int id)
    {
        return null;
    }

    private async Task<List<OrderDetail>> ConvertCartToOrderDetail(List<int>? carts)
    {
        List<OrderDetail> orderDetails = [];
        var listCart = await _context.Carts
            .Include(x=>x.Product)
            .Where(x => carts.Contains(x.Id)).ToListAsync();
        foreach (var item in listCart)
        {
            var orderDetail = new OrderDetail
            {
                ProductName = item.Product?.Name,
                PriceNow = item.PriceNow,
                TotalQuantity = item.Quantity,
                TotalPrice = item.Quantity * item.PriceNow
            };
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
        return new OrderItem
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            ReceiverInfo = order.ReceiverName + ", " + order.ReceiverPhone + ", " + order.ReceiverAddress,
            OrderDetailIds = order.OrderDetailIds,
            CreatedOn = order.CreatedOn,
            OrderStatus = order.OrderStatus,
            CustomerId = order.CustomerId
        };
    }

    private async Task<OrderDto> ConvertOrderDto(Order order)
    {
        var listOrderVouchers = await _context.OrderVouchers
            .Where(x => x.OrderId == order.Id).Select(x=>x.VoucherId).ToListAsync();
        var listVouchers = await _context.Vouchers.Where(x => listOrderVouchers.Contains(x.Id)).ToListAsync();
        List<VoucherItem> listVoucherItems = [];
        listVoucherItems.AddRange(listVouchers.Select(item => new VoucherItem
        {
            Id = item.Id,
            Title = item.Title,
            Code = item.Code,
            DiscountAmount = item.DiscountAmount,
            DiscountPercentage = item.DiscountPercentage,
            StartTime = item.StartTime,
            EndTime = item.EndTime
        }));
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
            Vouchers = listVoucherItems,
            CreatedOn = order.CreatedOn,
            OrderStatus = order.OrderStatus,
            CustomerId = order.CustomerId
        };
    }
}
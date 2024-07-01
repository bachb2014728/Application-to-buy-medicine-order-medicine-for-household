using System.ComponentModel.DataAnnotations;

namespace backend.Dto.Receiver;

public class ReceiverCreate
{
    [Required(ErrorMessage = "Tên người nhận không được để null.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Số điện thoai người nhận không được để null.")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Địa chỉ người nhận không được để null.")]
    public string Address { get; set; }
}
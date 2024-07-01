using backend.Dto;
using backend.Dto.Receiver;

namespace backend.Interface;

public interface IReceiver
{
    Task<IEnumerable<ReceiverItem>?> GetAll();
    Task<ReceiverDto?> Create(ReceiverCreate receiverCreate);
    Task<ApiObject> Checked(IsCheckedOfReceiver isCheckedOfReceiver);
    Task<ReceiverDto?> Update(ReceiverUpdate receiverUpdate, int id);
    Task<ApiObject?> Delete(int id);
}
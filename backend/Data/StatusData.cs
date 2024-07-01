using System.Runtime.Serialization;

namespace backend.Data
{
    public enum StatusData
    {
        [EnumMember(Value = "processing")]
        processing,
        [EnumMember(Value = "shipping")]
        shipping, 
        [EnumMember(Value = "delivered")]
        delivered, 
        [EnumMember(Value = "canceled")]
        canceled
    }
}
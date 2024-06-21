using backend.Dto;
using backend.Dto.Product.Contraindication;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interface;

public interface IContraindication
{
    Task<ContraindicationDto> GetOne(int id);
    Task<ContraindicationDto> Create(ContraindicationCreate item);
    Task<List<Contraindication>> GetAll();
    Task<ApiObject> DeleteOne(int id);
    Task<ContraindicationDto> Update(int id, ContraindicationUpdate item);
}
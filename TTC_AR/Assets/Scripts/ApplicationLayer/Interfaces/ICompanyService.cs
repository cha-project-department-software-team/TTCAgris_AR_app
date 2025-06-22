
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos.Company;
using Domain.Entities;

namespace ApplicationLayer.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyResponseDto> GetCompanyByIdAsync(int CompanyId);
        Task<List<CompanyResponseDto>> GetListCompanyAsync();
    }
}
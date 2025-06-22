
// Domain/Repositories/ICompanyRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICompanyRepository
    {
        Task<CompanyEntity> GetCompanyByIdAsync(int CompanyId);
        Task<List<CompanyEntity>> GetListCompanyAsync();
    }
}
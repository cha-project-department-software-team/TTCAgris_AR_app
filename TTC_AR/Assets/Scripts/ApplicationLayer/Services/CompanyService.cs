
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Company;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services
{
    //! Không bắt lỗi tại đây
    public class CompanyService : ICompanyService
    {
        private readonly CompanyUseCase _CompanyUseCase;
        public CompanyService(CompanyUseCase CompanyUseCase)
        {
            _CompanyUseCase = CompanyUseCase;
        }

        //! Dữ liệu trả về là Dto

        public async Task<CompanyResponseDto> GetCompanyByIdAsync(int CompanyId)
        {
            return await _CompanyUseCase.GetCompanyByIdAsync(CompanyId);
        }

        public async Task<List<CompanyResponseDto>> GetListCompanyAsync()
        {
            return await _CompanyUseCase.GetListCompanyAsync();
        }


    }

}
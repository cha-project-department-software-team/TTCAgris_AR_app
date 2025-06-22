
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Dtos;
using ApplicationLayer.Dtos.Grapper;
using ApplicationLayer.Interfaces;
using ApplicationLayer.UseCases;

namespace ApplicationLayer.Services
{

    //! Không bắt lỗi tại đây
    public class GrapperService : IGrapperService
    {
        private readonly GrapperUseCase _GrapperUseCase;
        public GrapperService(GrapperUseCase GrapperUseCase)
        {
            _GrapperUseCase = GrapperUseCase;
        }

        //! Tham số là Dto, Dữ liệu trả về là Dto
        public async Task<GrapperResponseDto> GetGrapperByIdAsync(int id)
        {
            return await _GrapperUseCase.GetGrapperByIdAsync(id);
        }
        public async Task<List<GrapperBasicDto>> GetListGrapperAsync(int grapperId)
        {
            return await _GrapperUseCase.GetListGrapperAsync(grapperId);
        }
        public async Task<bool> CreateNewGrapperAsync(int grapperId, GrapperRequestDto GrapperRequestDto)
        {
            return await _GrapperUseCase.CreateNewGrapperAsync(grapperId, GrapperRequestDto);
        }
        public async Task<bool> UpdateGrapperAsync(int GrapperId, GrapperRequestDto GrapperRequestDto)
        {
            return await _GrapperUseCase.UpdateGrapperAsync(GrapperId, GrapperRequestDto);
        }
        public async Task<bool> DeleteGrapperAsync(int GrapperId)
        {
            return await _GrapperUseCase.DeleteGrapperAsync(GrapperId);
        }
    }

}
using FinanceApp.Application.DTOs;

namespace FinanceApp.Application.Interfaces
{
        public interface IIncomeRecordService
        {
            Task<IEnumerable<IncomeRecordDto>> GetAllAsync();
            Task<IncomeRecordDto?> GetByIdAsync(int id);
            Task<int> CreateAsync(IncomeRecordDto dto);
            Task<bool> UpdateAsync(IncomeRecordDto dto);
            Task<bool> DeleteAsync(int id);
        }

}

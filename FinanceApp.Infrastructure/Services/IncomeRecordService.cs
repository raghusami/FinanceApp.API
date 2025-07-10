using FinanceApp.Application.DTOs;
using FinanceApp.Application.Interfaces;
using FinanceApp.Domain.Entities;
using FinanceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FinanceApp.Shared.Exceptions;

namespace FinanceApp.Infrastructure.Services;

public class IncomeRecordService : IIncomeRecordService
{
    private readonly AppDbContext _context;
    private readonly ILogger<IncomeRecordService> _logger;

    public IncomeRecordService(AppDbContext context, ILogger<IncomeRecordService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<IncomeRecordDto>> GetAllAsync()
    {
        try
        {
            return await _context.IncomeRecords
                .Select(r => new IncomeRecordDto
                {
                    Id = r.Id,
                    IncomeDate = r.IncomeDate,
                    IncomeSource = r.IncomeSource,
                    Amount = r.Amount,
                    Notes = r.Notes,
                    IncomeType = r.IncomeType
                }).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[IncomeRecordService] Error fetching income records.");
            throw;
        }
    }

    public async Task<IncomeRecordDto?> GetByIdAsync(int id)
    {
        try
        {
            var record = await _context.IncomeRecords.FindAsync(id);
            if (record == null) return null;

            return new IncomeRecordDto
            {
                Id = record.Id,
                IncomeDate = record.IncomeDate,
                IncomeSource = record.IncomeSource,
                Amount = record.Amount,
                Notes = record.Notes,
                IncomeType = record.IncomeType
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[IncomeRecordService] Error fetching income record with ID {id}.");
            throw;
        }
    }

    public async Task<int> CreateAsync(IncomeRecordDto dto)
    {
        try
        {
            if (dto.Amount <= 0)
                throw new FinanceValidationException("Amount must be greater than zero.");

            var record = new IncomeRecords
            {
                IncomeDate = dto.IncomeDate,
                IncomeSource = dto.IncomeSource,
                Amount = dto.Amount,
                Notes = dto.Notes,
                IncomeType = dto.IncomeType,
                CreatedAt = DateTime.Now
            };

            _context.IncomeRecords.Add(record);
            await _context.SaveChangesAsync();
            return record.Id;
        }
        catch (FinanceValidationException ex)
        {
            _logger.LogWarning(ex, "[IncomeRecordService] Validation failed while creating income record.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[IncomeRecordService] Error creating income record.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(IncomeRecordDto dto)
    {
        try
        {
            var record = await _context.IncomeRecords.FindAsync(dto.Id);
            if (record == null) return false;

            record.IncomeDate = dto.IncomeDate;
            record.IncomeSource = dto.IncomeSource;
            record.Amount = dto.Amount;
            record.Notes = dto.Notes;
            record.IncomeType = dto.IncomeType;
            record.UpdatedAt = DateTime.Now;

            _context.IncomeRecords.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[IncomeRecordService] Error updating income record ID {dto.Id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var record = await _context.IncomeRecords.FindAsync(id);
            if (record == null) return false;

            _context.IncomeRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[IncomeRecordService] Error deleting income record ID {id}.");
            throw;
        }
    }
}

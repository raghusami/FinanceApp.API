using FinanceApp.Application.DTOs;
using FinanceApp.Application.Interfaces;
using FinanceApp.Shared.Common.Responses;
using FinanceApp.Shared.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.API.Controllers
{

    /// <summary>
    /// Controller to manage income record operations.
    /// </summary>

    public class IncomeRecordsController : BaseController
    {
        private readonly IIncomeRecordService _service;
        private readonly ILogger<IncomeRecordsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeRecordsController"/> class.
        /// </summary>
        public IncomeRecordsController(IIncomeRecordService service, ILogger<IncomeRecordsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all income records.
        /// </summary>
        /// <returns>A list of income records.</returns>
        [HttpGet("IncomeGetAll")]
        [ProducesResponseType(typeof(ApiResponse<List<IncomeRecordDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var records = await _service.GetAllAsync();
                return SuccessResponseWithData(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[IncomeRecordsController] GetAll failed.");
                throw;
            }
        }

        /// <summary>
        /// Retrieves an income record by ID.
        /// </summary>
        /// <param name="id">The ID of the income record.</param>
        /// <returns>The income record if found; otherwise, not found.</returns>
        [HttpGet("IncomeGetById/{id}")]
        [ProducesResponseType(typeof(ApiResponse<IncomeRecordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var record = await _service.GetByIdAsync(id);
                if (record == null)
                    return NotFoundResponse("Income record not found.");
                return SuccessResponseWithData(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[IncomeRecordsController] GetById failed for ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Creates a new income record.
        /// </summary>
        /// <param name="dto">The income record DTO.</param>
        /// <returns>The created income record.</returns>
        [HttpPost("IncomeCreate")]
        [ProducesResponseType(typeof(ApiResponse<IncomeRecordDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] IncomeRecordDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequestResponse("Invalid income data.");

                var id = await _service.CreateAsync(dto);
                var created = await _service.GetByIdAsync(id);
                if (created == null)
                    return InternalServerErrorResponse("Failed to retrieve the created income record.");

                return SuccessResponseWithData(created, "Income record created successfully.");
            }
            catch (FinanceValidationException ex)
            {
                _logger.LogWarning(ex, "[IncomeRecordsController] Validation failed during Create.");
                return BadRequestResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[IncomeRecordsController] Create failed.");
                throw;
            }
        }

        /// <summary>
        /// Updates an existing income record.
        /// </summary>
        [HttpPut("IncomeUpdate/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] IncomeRecordDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequestResponse("ID mismatch between route and body.");

                var success = await _service.UpdateAsync(dto);
                if (!success)
                    return NotFoundResponse("Income record not found.");

                return SuccessResponse("Income record updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[IncomeRecordsController] Update failed for ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Deletes an income record by ID.
        /// </summary>
        [HttpDelete("IncomeDelete/{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteAsync(id);
                if (!success)
                    return NotFoundResponse("Income record not found.");

                return DeleteSuccessResponse("Income record deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[IncomeRecordsController] Delete failed for ID {Id}", id);
                throw;
            }
        }
    }

}
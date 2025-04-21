using CRMSystem.WebAPI.DTOs.School.Contracts;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/contracts")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContractController(ContractService contractService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await contractService.GetAllAsync();
            return Ok(contracts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contract = await contractService.GetByIdAsync(id);
            return contract is null ? NotFound() : Ok(contract);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractDto dto)
        {
            var created = await contractService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateContractDto dto)
        {
            var updated = await contractService.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await contractService.DeleteAsync(id);
            return NoContent();
        }
    }
}
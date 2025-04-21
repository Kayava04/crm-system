using CRMSystem.WebAPI.Auth;
using CRMSystem.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/finance-report")]
    public class FinanceReportController(
        FinanceReportService reportService,
        ILogger<FinanceReportController> logger) : ControllerBase
    {
        [HttpGet("summary")]
        [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
        public async Task<IActionResult> GetSummary()
        {
            logger.LogInformation("Generating finance summary...");
            var report = await reportService.GenerateReportAsync();
            
            logger.LogInformation($"Finance summary report generated successfully: {report}.");
            return Ok(report);
        }
    }
}
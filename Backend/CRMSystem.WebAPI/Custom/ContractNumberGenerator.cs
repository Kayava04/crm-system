using CRMSystem.WebAPI.Core;
using CRMSystem.WebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.WebAPI.Custom
{
    public class ContractNumberGenerator(SchoolDbContext context)
        : IContractNumberGenerator
    {
        public async Task<string> Generate(DateTime signDate)
        {
            var datePart = signDate.ToString("yyyyMMdd");

            var contractNumbers = await context.Contracts
                .Where(c => c.ContractNumber.StartsWith($"CTR-{datePart}"))
                .Select(c => c.ContractNumber)
                .ToListAsync();

            var sequence = contractNumbers
                .Select(c =>
                {
                    var parts = c.Split('-');
                    return parts.Length == 3 && int.TryParse(parts[2], out var seq) ? seq : 0;
                })
                .ToList();

            var lastSequence = sequence.Any() ? sequence.Max() : 0;
            
            var newSequence = lastSequence + 1;
            return $"CTR-{datePart}-{newSequence:D4}";
        }
    }
}
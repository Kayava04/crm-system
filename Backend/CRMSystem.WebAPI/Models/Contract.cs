namespace CRMSystem.WebAPI.Models
{
    public class Contract
    {
        public Guid Id { get; private set; }
        public string ContractNumber { get; private set; }
        public DateTime SignDate { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public Guid StudentId { get; private set; }

        private Contract(Guid id, string contractNumber, DateTime signDate, decimal paymentAmount, Guid studentId)
        {
            Id = id;
            ContractNumber = contractNumber;
            SignDate = signDate;
            PaymentAmount = paymentAmount;
            StudentId = studentId;
        }

        public static Contract Create(Guid id, string contractNumber, DateTime signDate, decimal paymentAmount, Guid studentId) =>
            new(id, contractNumber, signDate, paymentAmount, studentId);
        
        public void Update(DateTime signDate, decimal paymentAmount) =>
            (SignDate, PaymentAmount) = (signDate, paymentAmount);
    }
}
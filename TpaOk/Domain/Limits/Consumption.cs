using System;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class Consumption
    {
        public int ConsumptionId { get; private set; }
        public int PolicyId { get; private set; }
        public int InsuredId { get; private set; }
        public string CaseNumber { get; private set; }
        public string ServiceCode { get; private set; }
        public DateTime ConsumptionDate { get; private set; }
        public Money ConsumedAmount { get; private set; }
        public int ConsumedQuantity { get; private set; }
        
        
        public Consumption(CaseServiceCostSplit caseService)
            : this(caseService.PolicyId, caseService.InsuredId, caseService.CaseNumber, caseService.ServiceCode, caseService.Date,
                caseService.AmountLimitConsumption, caseService.QtLimitConsumption)
        {}

        public Consumption(int policyId, int insuredId, string caseNumber, string serviceCode, DateTime consumptionDate, Money consumedAmount, int consumedQuantity)
        {
            PolicyId = policyId;
            InsuredId = insuredId;
            CaseNumber = caseNumber;
            ServiceCode = serviceCode;
            ConsumptionDate = consumptionDate;
            ConsumedAmount = consumedAmount;
            ConsumedQuantity = consumedQuantity;
        }
    }
}
using System;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class Consumption
    {
        public int ConsumptionId { get; private set; }
        public LimitConsumptionContainer ConsumptionContainer { get; private set; }
        public string CaseNumber { get; private set; }
        public Guid ServiceId { get; private set; }
        public DateTime ConsumptionDate { get; private set; }
        public Money ConsumedAmount { get; private set; }
        public int ConsumedQuantity { get; private set; }

        //required by EF
        protected Consumption()
        {
        }

        public Consumption(LimitConsumptionContainer consumptionContainer , CaseServiceCostSplit caseService)
        {
            ConsumptionContainer = consumptionContainer;
            ServiceId = caseService.CaseServiceId;
            CaseNumber = caseService.CaseNumber;
            ConsumptionDate = caseService.Date;
            ConsumedAmount = caseService.AmountLimitConsumption;
            ConsumedQuantity = caseService.QtLimitConsumption;
        }

        public Consumption(LimitConsumptionContainer consumptionContainer, string caseNumber, Guid serviceId, DateTime consumptionDate, Money consumedAmount, int consumedQuantity)
        {
            ConsumptionContainer = consumptionContainer;
            CaseNumber = caseNumber;
            ServiceId = serviceId;
            ConsumptionDate = consumptionDate;
            ConsumedAmount = consumedAmount;
            ConsumedQuantity = consumedQuantity;
        }
    }
}
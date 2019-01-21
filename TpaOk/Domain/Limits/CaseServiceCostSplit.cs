using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class CaseServiceCostSplit
    {
        public Guid CaseServiceId { get; private set; }
        
        public Money InsuredCost { get; private set; }
        public Money TuCost { get; private set; }
        public Money AmountLimitConsumption { get; private set; }
        public int QtLimitConsumption { get; private set; }
        public Money TotalCost => Price * Qt;
        
        public string ServiceCode { get; private set; }
        public decimal Qt { get; private set; }
        public DateTime Date { get; private set; }
        public Money Price { get; private set; }
        
        public int PolicyId { get; private set; }
        public string CaseNumber { get; private set; }
        public int InsuredId { get; private set; }

        public CaseServiceCostSplit(Case @case, CaseService caseService)
        {
            CaseServiceId = caseService.ServiceId;
            
            InsuredCost = Money.Euro(0);
            TuCost = caseService.Cost;
            AmountLimitConsumption = Money.Euro(0);
            QtLimitConsumption = 0;
            
            ServiceCode = caseService.ServiceCode;
            Qt = caseService.Qt;
            Date = caseService.Date;
            Price = caseService.Price;

            PolicyId = @case.PolicyId;
            CaseNumber = @case.Number;
            InsuredId = @case.InsuredId;
        }
        
        public void ApplyCoverageCheck(CoverageCheckPolicy coverageCheckPolicy)
        {
            var coverageCheckResult = coverageCheckPolicy.Apply(this);
            
            if (!coverageCheckResult.IsCovered)
            {
                TuCost -= coverageCheckResult.NotCoveredAmount;
                InsuredCost += coverageCheckResult.NotCoveredAmount;
            }
        }


        public void ApplyCoPayment(CoPaymentPolicy coPaymentPolicy)
        {
            var coPaymentResult = coPaymentPolicy.Apply(this);
            if (InsuredCost != TotalCost && coPaymentResult.NotCoveredAmount > Money.Euro(0))
            {
                InsuredCost += coPaymentResult.NotCoveredAmount;
                TuCost -= coPaymentResult.NotCoveredAmount;
            }
        }

        public void ApplyLimit(LimitsPolicy limitPolicy)
        {
            var limitApplicationResult = limitPolicy.Apply(this);
            if (InsuredCost != TotalCost && limitApplicationResult.IsApplied)
            {
                InsuredCost += limitApplicationResult.NotCoveredAmount;
                TuCost -= limitApplicationResult.NotCoveredAmount;
                AmountLimitConsumption += limitApplicationResult.LimitConsumption;
            }
        }
    }

    
}
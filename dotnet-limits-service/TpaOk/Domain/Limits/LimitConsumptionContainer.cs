using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class LimitConsumptionContainer
    {
        public Guid Id { get; private set; }
        
        public int PolicyId { get; private set; }
        public string ServiceCode { get; private set; }
        
        public List<Consumption> Consumptions { get; private set; }
        
        public byte[] RowVersion { get; private set; }

        //used by EF
        protected LimitConsumptionContainer()
        {
        }


        public LimitConsumptionContainer(
            int policyId,
            string serviceCode)
        {
            PolicyId = policyId;
            ServiceCode = serviceCode;
            
            Consumptions = new List<Consumption>();
        }

        public void ReserveLimitsFor(CaseServiceCostSplit caseService, Money amountConsumption, int qtConsumption)
        {
            ReserveLimitsFor(caseService.CaseNumber, caseService.CaseServiceId, caseService.Date, amountConsumption, qtConsumption);
        }
        
        public void ReserveLimitsFor(string caseNumber, Guid serviceId, DateTime consumptionDate, Money consumedAmount, int consumedQuantity)
        {
            Consumptions.Add(new Consumption(this, caseNumber, serviceId, consumptionDate, consumedAmount, consumedQuantity));
        }

        public (Money, int) CurrentConsumption()
        {
            var amtConsumption = Consumptions.Aggregate(Money.Euro(0), (s, i) => s + i.ConsumedAmount);
            var qtConsumption = Consumptions.Aggregate(0, (s, i) => s + i.ConsumedQuantity);

            return (amtConsumption, qtConsumption);
        }
    }

    
    public class SharedConsumptionContainerForService : LimitConsumptionContainer
    {
        public Period Period { get; private set; }
        //used by EF
        protected SharedConsumptionContainerForService() : base()
        {
        }

        public SharedConsumptionContainerForService(int policyId, string serviceCode, Period period) 
            : base(policyId, serviceCode)
        {
            Period = period;
        }
    }

    public class IndividualInsuredConsumptionContainerForService : LimitConsumptionContainer
    {
        public int InsuredId { get; private set; }
        public Period Period { get; private set; }

        //used by EF
        protected IndividualInsuredConsumptionContainerForService() : base()
        {
        }
        
        public IndividualInsuredConsumptionContainerForService(int policyId, string serviceCode, int insuredId, Period period) : base(policyId, serviceCode)
        {
            InsuredId = insuredId;
            Period = period;
        }
    }

    public class CaseConsumptionContainerForService : LimitConsumptionContainer
    {
        public string CaseNumber { get; private set; }
        
        public CaseConsumptionContainerForService(int policyId, string serviceCode, string caseNumber) : base(policyId, serviceCode)
        {
            CaseNumber = caseNumber;
        }
    }
}
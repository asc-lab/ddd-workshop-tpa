using System;
using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class CalculateCostSplitAndReserveLimitsCommandHandler
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly ILimitConsumptionsRepository _limitConsumptionsRepositoryRepository;

        public CalculateCostSplitAndReserveLimitsCommandHandler(IPolicyRepository policyRepository, ILimitConsumptionsRepository limitConsumptionsRepositoryRepository)
        {
            _policyRepository = policyRepository;
            _limitConsumptionsRepositoryRepository = limitConsumptionsRepositoryRepository;
        }

        public CalculateCostSplitAndReserveLimitsResult Handle(CalculateCostSplitAndReserveLimitsCommand cmd)
        {
            var costSplit = CalculateCostSplitAndReserveLimitsResult.Initial(cmd.Case);

            foreach (var caseService in cmd.Case.Services)
            {
                var policyVersionAtServiceDate =
                    _policyRepository.GetVersionValidAt(cmd.Case.PolicyId, caseService.Date);
             
                var serviceCoveredPolicy = new ServiceCoveredPolicy(policyVersionAtServiceDate);
                var coPaymentPolicy = new CoPaymentPolicy(policyVersionAtServiceDate);
                var limitPolicy = new LimitsPolicy(policyVersionAtServiceDate, _limitConsumptionsRepositoryRepository);
                
                var serviceCoveredPolicyResult = serviceCoveredPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(caseService, serviceCoveredPolicyResult);

                var coPaymentApplicationResult = coPaymentPolicy.Apply(cmd.Case, caseService);
                costSplit.Apply(caseService, coPaymentApplicationResult);

                var limitApplicationResult = limitPolicy.Apply(cmd.Case, caseService, costSplit);
                costSplit.Apply(caseService, limitApplicationResult);
                if (limitApplicationResult.IsApplied)
                {
                    _limitConsumptionsRepositoryRepository.Add(new Consumption(cmd.Case, caseService,
                        costSplit.AmountLimitConsumption, costSplit.QtLimitConsumption));
                }
            }



            return costSplit;
        }
    }

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
        
        
        public Consumption(Case cmdCase, CaseService caseService, Money amountConsumed, int qtConsumed)
            : this(cmdCase.PolicyId, cmdCase.InsuredId, cmdCase.Number, caseService.ServiceCode, caseService.Date,
                amountConsumed, qtConsumed)
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
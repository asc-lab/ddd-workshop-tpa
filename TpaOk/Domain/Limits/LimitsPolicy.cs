using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class LimitsPolicy
    {
        private readonly PolicyVersion _policyVersion;
        private readonly ILimitConsumptionsRepository _limitConsumptionsRepository;

        public LimitsPolicy(PolicyVersion policyVersion, ILimitConsumptionsRepository limitConsumptionsRepository)
        {
            _policyVersion = policyVersion;
            _limitConsumptionsRepository = limitConsumptionsRepository;
        }
        
        public LimitsApplicationResult Apply(CaseServiceCostSplit caseService)
        {
            if (_policyVersion == null)
            {
                return LimitsApplicationResult.NoApplied();                
            }

            var limit = _policyVersion.LimitFor(caseService.ServiceCode);
            if (limit == null)
            {
                return LimitsApplicationResult.NoApplied();
            }

            var currentLimitConsumption = _limitConsumptionsRepository.GetLimitConsumption
            (
                caseService,
                limit,
                limit.CalculatePeriod(caseService.Date, _policyVersion)
            );
            var limitCalculation = limit.Calculate(caseService, currentLimitConsumption);
            
            return LimitsApplicationResult.Applied(limitCalculation);
        }
    }

    public class LimitsApplicationResult
    {
        public bool IsApplied { get; }
        public Money LimitConsumption { get; }
        public Money NotCoveredAmount { get; }

        public static LimitsApplicationResult NoApplied()
        {
            return new LimitsApplicationResult(false);
        }
        
        public static LimitsApplicationResult Applied(LimitCalculation limitCalculation)
        {
          return new LimitsApplicationResult(true, limitCalculation.LimitConsumption, limitCalculation.NotCoveredAmount);
        }
        
        private LimitsApplicationResult(bool isApplied)
        {
            IsApplied = isApplied;
        }

        private LimitsApplicationResult(bool isApplied, Money limitConsumption, Money notCoveredAmount)
        {
            IsApplied = isApplied;
            LimitConsumption = limitConsumption;
            NotCoveredAmount = notCoveredAmount;
        }
    }
}
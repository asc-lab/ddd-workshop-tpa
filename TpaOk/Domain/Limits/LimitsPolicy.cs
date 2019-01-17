using NodaMoney;

namespace TpaOk.Domain.Limits
{
    public class LimitsPolicy
    {
        private readonly PolicyVersion _policyVersion;

        public LimitsPolicy(PolicyVersion policyVersion)
        {
            _policyVersion = policyVersion;
        }
        
        public LimitsApplicationResult Apply(Case @case, CaseService caseService,
            CalculateCostSplitAndReserveLimitsResult costSplit)
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

            var limitCalculation = limit.Calculate(costSplit);
            
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
            this.IsApplied = isApplied;
        }

        private LimitsApplicationResult(bool isApplied, Money limitConsumption, Money notCoveredAmount)
        {
            IsApplied = isApplied;
            LimitConsumption = limitConsumption;
            NotCoveredAmount = notCoveredAmount;
        }
    }
}
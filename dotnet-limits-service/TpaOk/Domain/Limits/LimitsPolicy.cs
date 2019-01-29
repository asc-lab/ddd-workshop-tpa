using NodaMoney;
using TpaOk.Commands;

namespace TpaOk.Domain.Limits
{
    public class LimitsPolicy
    {
        private readonly PolicyVersion _policyVersion;
        private readonly ILimitConsumptionContainerRepository _limitConsumptionContainers;

        public LimitsPolicy(PolicyVersion policyVersion, ILimitConsumptionContainerRepository limitConsumptionContainers)
        {
            _policyVersion = policyVersion;
            _limitConsumptionContainers = limitConsumptionContainers;
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

            var limitConsumptionContainer = FindOrCreateMatchingConsumptionContainer(caseService, limit);


            var limitCalculation = limit.Calculate
            (
                caseService, 
                limitConsumptionContainer.CurrentConsumption()
            );
            
            limitConsumptionContainer.ReserveLimitsFor(caseService, limitCalculation.LimitConsumption, 0);
            
            return LimitsApplicationResult.Applied(limitCalculation);
        }

        private LimitConsumptionContainer FindOrCreateMatchingConsumptionContainer(CaseServiceCostSplit caseService,
            Limit limit)
        {
            var consumptionPeriod = limit.CalculatePeriod(caseService.Date, _policyVersion);
            var limitConsumptionContainer = _limitConsumptionContainers.GetLimitConsumptionContainer
            (
                caseService,
                limit,
                consumptionPeriod
            );

            if (limitConsumptionContainer == null)
            {
                limitConsumptionContainer =
                    new LimitConsumptionContainerFactory().Create(caseService, limit, consumptionPeriod);
                _limitConsumptionContainers.Add(limitConsumptionContainer);
            }

            return limitConsumptionContainer;
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
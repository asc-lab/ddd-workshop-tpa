namespace TpaOk.Domain.Limits
{
    public class LimitConsumptionContainerFactory
    {
        public LimitConsumptionContainer Create(CaseServiceCostSplit caseService, Limit limit,
            Period period)
        {
            if (limit.LimitPeriod is PerCaseLimitPeriod)
            {
                return CreatePerCaseConsumptionContainerForService(caseService);
            }

            if (limit.Shared)
            {
                return CreatePerPolicyContainerForService(caseService, period);
            }

            return CreatePerInsuredContainerForService(caseService, period);
        }

        private LimitConsumptionContainer CreatePerInsuredContainerForService(CaseServiceCostSplit caseService, Period period)
        {
            return new IndividualInsuredConsumptionContainerForService
            (
                caseService.PolicyId,
                caseService.ServiceCode,
                caseService.InsuredId,
                period
            );
        }

        private LimitConsumptionContainer CreatePerPolicyContainerForService(CaseServiceCostSplit caseService, Period period)
        {
            return new SharedConsumptionContainerForService
            (
                caseService.PolicyId,
                caseService.ServiceCode,
                period
            );
        }

        private LimitConsumptionContainer CreatePerCaseConsumptionContainerForService(CaseServiceCostSplit caseService)
        {
            return new CaseConsumptionContainerForService
            (
                caseService.PolicyId,
                caseService.ServiceCode,
                caseService.CaseNumber
            );
        }
    }
}
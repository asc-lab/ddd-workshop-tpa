using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests
{
    public class ManyServicesOnCaseTests
    {
        private ILimitConsumptionsRepository limitConsumptionsRepository;
        private CalculateCostSplitAndReserveLimitsCommandHandler cmdHandler;

        public ManyServicesOnCaseTests()
        {
            limitConsumptionsRepository = new MockLimitConsumptionRepository();
            cmdHandler = new CalculateCostSplitAndReserveLimitsCommandHandler(new MockPolicyRepository(), limitConsumptionsRepository);
        }

        [Fact]
        public void CaseWithTwoServices_CoPaymentApplied_LimitNotApplied_NoPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 9,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(100),
                        Qt = 1
                    },
                    new CaseService
                    {
                        ServiceCode = "KONS_LARYNGOLOG",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(200),
                        Qt = 1
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(10), result.CostSplitForCaseService(medCase.Services[0]).InsuredCost);
            Assert.Equal(Money.Euro(90), result.CostSplitForCaseService(medCase.Services[0]).TuCost);
            Assert.Equal(Money.Euro(100), result.CostSplitForCaseService(medCase.Services[0]).TotalCost);
            Assert.Equal(Money.Euro(90), result.CostSplitForCaseService(medCase.Services[0]).AmountLimitConsumption);
            
            Assert.Equal(Money.Euro(20), result.CostSplitForCaseService(medCase.Services[1]).InsuredCost);
            Assert.Equal(Money.Euro(180), result.CostSplitForCaseService(medCase.Services[1]).TuCost);
            Assert.Equal(Money.Euro(200), result.CostSplitForCaseService(medCase.Services[1]).TotalCost);
            Assert.Equal(Money.Euro(180), result.CostSplitForCaseService(medCase.Services[1]).AmountLimitConsumption);
        }
        
        [Fact]
        public void CaseWithTwoServices_CoPaymentApplied_LimitApplied_CausePreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 9,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(100),
                        Qt = 1
                    },
                    new CaseService
                    {
                        ServiceCode = "KONS_LARYNGOLOG",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(200),
                        Qt = 1
                    }
                }
            };
            
            //and
            limitConsumptionsRepository.Add(new Consumption(9,1,"CASE8777","KONS_INTERNISTA",new DateTime(2019,1,9),Money.Euro(950),0));

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(50), result.CostSplitForCaseService(medCase.Services[0]).InsuredCost);
            Assert.Equal(Money.Euro(50), result.CostSplitForCaseService(medCase.Services[0]).TuCost);
            Assert.Equal(Money.Euro(100), result.CostSplitForCaseService(medCase.Services[0]).TotalCost);
            Assert.Equal(Money.Euro(50), result.CostSplitForCaseService(medCase.Services[0]).AmountLimitConsumption);
            
            Assert.Equal(Money.Euro(20), result.CostSplitForCaseService(medCase.Services[1]).InsuredCost);
            Assert.Equal(Money.Euro(180), result.CostSplitForCaseService(medCase.Services[1]).TuCost);
            Assert.Equal(Money.Euro(200), result.CostSplitForCaseService(medCase.Services[1]).TotalCost);
            Assert.Equal(Money.Euro(180), result.CostSplitForCaseService(medCase.Services[1]).AmountLimitConsumption);
        }
        
    }
}
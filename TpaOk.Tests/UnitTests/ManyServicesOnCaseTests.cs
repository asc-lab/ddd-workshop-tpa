using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests.UnitTests
{
    public class ManyServicesOnCaseTests
    {
        private IDataStore dataStore;
        private CalculateCostSplitAndReserveLimitsHandler cmdHandler;

        public ManyServicesOnCaseTests()
        {
            dataStore = new MockDataStore();
            cmdHandler = new CalculateCostSplitAndReserveLimitsHandler(dataStore);
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
            var container = new IndividualInsuredConsumptionContainerForService
            (
                9,"KONS_INTERNISTA",1,Period.Between(new DateTime(2019,1,1), new DateTime(2019,12,31))
            );
            container.ReserveLimitsFor("CASE8777", Guid.NewGuid(), new DateTime(2019, 1, 9), Money.Euro(950), 0);
            dataStore.LimitConsumptionContainers.Add(container);

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
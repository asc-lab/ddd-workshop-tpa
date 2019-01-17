using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests
{
    public class LimitsTests
    {
        private CalculateCostSplitAndReserveLimitsCommandHandler cmdHandler =
            new CalculateCostSplitAndReserveLimitsCommandHandler(new MockPolicyRepository(), new MockLimitConsumptionRepository());
        
        [Fact]
        public void AmountLimitNotExceeded_NoPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 6,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(100),
                        Qt = 1
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(0), result.InsuredCost);
            Assert.Equal(Money.Euro(100), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            Assert.Equal(Money.Euro(100), result.LimitConsumption);
        }
        
        [Fact]
        public void AmountLimitExceeded_NoPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 6,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(600),
                        Qt = 1
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(500), result.TuCost);
            Assert.Equal(Money.Euro(600), result.TotalCost);
            Assert.Equal(Money.Euro(500), result.LimitConsumption);
        }
        
        [Fact]
        public void AmountLimitExceeded_WithPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 7,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(200),
                        Qt = 1
                    }
                }
            };
            
            //TODO: previous consumption 400

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(100), result.TuCost);
            Assert.Equal(Money.Euro(200), result.TotalCost);
            Assert.Equal(Money.Euro(100), result.LimitConsumption); //TODO: ? czy to ma byc total zuzycie czy tylko z tego case'a
        }
    }
}
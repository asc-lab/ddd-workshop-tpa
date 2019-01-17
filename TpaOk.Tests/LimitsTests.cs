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
            new CalculateCostSplitAndReserveLimitsCommandHandler(new MockPolicyRepository());
        
        [Fact]
        public void AmountLimitNotExceeded()
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
        public void AmountLimitExceeded()
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
    }
}
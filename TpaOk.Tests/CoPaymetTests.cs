using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests
{
    public class CoPaymetTests
    {
        private CalculateCostSplitAndReserveLimitsCommandHandler cmdHandler =
            new CalculateCostSplitAndReserveLimitsCommandHandler(new MockPolicyRepository(), new MockLimitConsumptionRepository());
        
        [Fact]
        public void NoCopayment_TuPaysAll()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 1,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(50),
                        Qt = 2
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(0), result.InsuredCost);
            Assert.Equal(Money.Euro(100), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
        
        [Fact]
        public void TenPercentCopayment_Insured10PercentPart()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 3,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(50),
                        Qt = 2
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(10), result.InsuredCost);
            Assert.Equal(Money.Euro(90), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
        
        [Fact]
        public void FiveteenAmountCopayment_Insured30AmountPart()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 4,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(50),
                        Qt = 2
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(30), result.InsuredCost);
            Assert.Equal(Money.Euro(70), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
        
        [Fact]
        public void Over110Copayment_Insured100AmountPart()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 5,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(50),
                        Qt = 2
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(0), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
    }
}
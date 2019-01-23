using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Commands;
using Xunit;

namespace TpaOk.Tests.UnitTests
{
    public class ServiceCoverageTests
    {   
        private CalculateCostSplitAndReserveLimitsHandler cmdHandler =
            new CalculateCostSplitAndReserveLimitsHandler(new MockDataStore());
        
        [Fact]
        public void NoLimitsAndNoPreviousConsumption_TuPaysAll()
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
            
        }
        
        [Fact]
        public void NoLimitsAndNoPreviousConsumptionButServiceNotCoverByPolicy_InsuredPaysAll()
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
                        ServiceCode = "KONS_INTERNISTA_X",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(100),
                        Qt = 1
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
        
        [Fact]
        public void NoLimitsAndNoPreviousConsumptionButInsuredNotCoverByPolicy_InsuredPaysAll()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 2,
                PolicyId = 1,
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
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(0), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
        
        [Fact]
        public void NoLimitsAndNoPreviousConsumptionButNoPolicyVersionAtServiceDate_InsuredPaysAll()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 2,
                PolicyId = 2,
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
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(0), result.TuCost);
            Assert.Equal(Money.Euro(100), result.TotalCost);
            
        }
    }
}
using System;
using System.Collections.Generic;
using NodaMoney;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests
{
    public class LimitsTests
    {
        private ILimitConsumptionsRepository limitConsumptionsRepository;
        private CalculateCostSplitAndReserveLimitsCommandHandler cmdHandler;

        public LimitsTests()
        {
            limitConsumptionsRepository = new MockLimitConsumptionRepository();
            cmdHandler = new CalculateCostSplitAndReserveLimitsCommandHandler(new MockPolicyRepository(), limitConsumptionsRepository);
        }
        
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
            Assert.Equal(Money.Euro(100), result.AmountLimitConsumption);
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
            Assert.Equal(Money.Euro(500), result.AmountLimitConsumption);
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
            
            //and
            limitConsumptionsRepository.Add(new Consumption(7,1,"CASE8777","KONS_INTERNISTA",new DateTime(2019,1,9),Money.Euro(400),0));

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(100), result.InsuredCost);
            Assert.Equal(Money.Euro(100), result.TuCost);
            Assert.Equal(Money.Euro(200), result.TotalCost);
            Assert.Equal(Money.Euro(100), result.AmountLimitConsumption); //TODO: ? czy to ma byc total zuzycie czy tylko z tego case'a
        }
        
        [Fact]
        public void AmountLimitNotExceededDueToCoPaymentApplied_NoPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 8,
                Services = new List<CaseService>
                {
                    new CaseService
                    {
                        ServiceCode = "KONS_INTERNISTA",
                        Date = new DateTime(2019,1,10),
                        Price = Money.Euro(520),
                        Qt = 1
                    }
                }
            };

            //when
            var result = cmdHandler.Handle(new CalculateCostSplitAndReserveLimitsCommand(medCase));

            //then
            Assert.Equal(Money.Euro(52), result.InsuredCost);
            Assert.Equal(Money.Euro(468), result.TuCost);
            Assert.Equal(Money.Euro(520), result.TotalCost);
            Assert.Equal(Money.Euro(468), result.AmountLimitConsumption); //TODO: ? czy to ma byc total zuzycie czy tylko z tego case'a
        }
        
        [Fact]
        public void AmountLimitExceededAndCoPaymentApplied_NoPreviousConsumptions()
        {
            //given
            var medCase = new Case
            {
                Number = "CASE_1",
                InsuredId = 1,
                PolicyId = 8,
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
            Assert.Equal(Money.Euro(500), result.AmountLimitConsumption); //TODO: ? czy to ma byc total zuzycie czy tylko z tego case'a
        }
    }
}
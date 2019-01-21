using System;
using TpaOk.Domain.Limits;
using Xunit;

namespace TpaOk.Tests.UnitTests
{
    public class PolicyYearLimitPeriodTests
    {
        [Fact]
        public void ServiceDateInsideFirstPolicyYear_PeriodStartEqualToPolicyStart_PeriodEndsYearAfterThat()
        {
            //given
            var policy = new PolicyVersion
            {
                PolicyId = 1,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2019,12,31)
            };
            
            var serviceDate = new DateTime(2019, 5,1);
            
            //when
            var limitPeriod = new PolicyYearLimitPeriod().Calculate(serviceDate, policy);
            
            //then
            Assert.Equal(Period.Between(new DateTime(2019,1,1),new DateTime(2019,12,31)), limitPeriod);
        }
        
        
        [Fact]
        public void ServiceDateInsideSecondPolicyYear_PeriodStartEqualToPolicyStart_PeriodEndsYearAfterThat()
        {
            //given
            var policy = new PolicyVersion
            {
                PolicyId = 1,
                PolicyFrom = new DateTime(2019,1,1),
                PolicyTo = new DateTime(2020,12,31)
            };
            
            var serviceDate = new DateTime(2020, 5,1);
            
            //when
            var limitPeriod = new PolicyYearLimitPeriod().Calculate(serviceDate, policy);
            
            //then
            Assert.Equal(Period.Between(new DateTime(2020,1,1),new DateTime(2020,12,31)), limitPeriod);
        }
        
        [Fact]
        public void LeapYear()
        {
            //given
            var policy = new PolicyVersion
            {
                PolicyId = 1,
                PolicyFrom = new DateTime(2020,2,29),
                PolicyTo = new DateTime(2020,2,29).AddYears(1).AddDays(-1)
            };
            
            var serviceDate = new DateTime(2020, 5,1);
            
            //when
            var limitPeriod = new PolicyYearLimitPeriod().Calculate(serviceDate, policy);
            
            //then
            Assert.Equal(Period.Between(new DateTime(2020,2,29),new DateTime(2021,2,27)), limitPeriod);
        }
    }
}
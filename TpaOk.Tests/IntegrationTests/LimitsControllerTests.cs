using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Newtonsoft.Json;
using NodaMoney;
using TpaOk.Commands;
using Xunit;

namespace TpaOk.Tests.IntegrationTests
{
    public class LimitsControllerTests
    {
        [Fact]
        public async Task CallCostSplitAndReserveLimits_CoPaymentApplied_LimitNotExceeded()
        {
            using (var system = SystemUnderTest.ForStartup<Startup>())
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
                
                // when
                var httpResult = await system.Scenario(_ =>
                {
                    _.Post
                        .Json(new CalculateCostSplitAndReserveLimitsCommand(medCase))
                        .ToUrl("/api/Limits/calculateAndReserveLimits");
                });

                var costSplitResult = httpResult.ResponseBody.ReadAsJson<CalculateCostSplitAndReserveLimitsResult>();
                
                // then
                Assert.NotNull(costSplitResult);     
                Assert.Equal(Money.Euro(10), costSplitResult.InsuredCost);
                Assert.Equal(Money.Euro(90), costSplitResult.TuCost);
                Assert.Equal(Money.Euro(100), costSplitResult.TotalCost);
                Assert.Equal(Money.Euro(90), costSplitResult.AmountLimitConsumption);
                
            }
        }
    }
}
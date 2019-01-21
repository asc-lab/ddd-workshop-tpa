package pl.asc.tparegistercase.command.rejectservice

import pl.asc.tparegistercase.CaseBuilder
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseHandler
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.CostReportServiceImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class RejectServiceInCaseHandlerSpec extends Specification {

    def "should finish registering case with 2 services and 1 rejected"() {
        given:
            def repository = new CaseRepositoryTestImpl()
            def costReportServiceImpl = new CostReportServiceImpl()
            def mspPriceServiceTestImpl = new MspPriceServiceTestImpl()
            def aCase = repository.save(CaseBuilder.createCase('93010267675', 'Jan', 'Kowalski', 'POL1', 'Warsaw'))
        when:
            new AcceptServiceInCaseHandler(repository, costReportServiceImpl, mspPriceServiceTestImpl).handle(new AcceptServiceInCaseCommand(
                    caseNumber: aCase.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            new AcceptServiceInCaseHandler(repository, costReportServiceImpl, mspPriceServiceTestImpl).handle(new AcceptServiceInCaseCommand(
                    caseNumber: aCase.getCaseNumber(),
                    serviceCode: 'DENTYSTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            new RejectServiceInCaseHandler(repository, costReportServiceImpl)
                    .handle(new RejectServiceInCaseCommand(aCase.getCaseNumber(), 2, "To Expensive"))

        then:
            aCase.rejectedServices.size() == 1
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(10)
    }

}

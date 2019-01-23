package pl.asc.tparegistercase.command.updateserviceincase

import pl.asc.tparegistercase.CaseBuilder
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseHandler
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.CostReportServiceImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class UpdateServiceInCaseHandlerSpec extends Specification {


    def "should update service in case with order number 2"() {
        given:
            def repository = new CaseRepositoryTestImpl()
            def costReportServiceImpl = new CostReportServiceImpl()
            def mspPriceServiceTestImpl = new MspPriceServiceTestImpl()
            def aCase = repository.save(CaseBuilder.createCase('93010267675', 'Jan', 'Kowalski', 'POL1', 'Warsaw'))
        when:
            new AcceptServiceInCaseHandler(repository, costReportServiceImpl, mspPriceServiceTestImpl).handle(new AcceptServiceInCaseCommand(
                    caseNumber: aCase.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 2,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))


            new UpdateServiceInCaseHandler(repository, costReportServiceImpl, mspPriceServiceTestImpl).handle(new UpdateServiceInCaseCommand(
                    caseNumber: aCase.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now(),
                    orderNumber: 1))
        then:
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(10)

    }
}
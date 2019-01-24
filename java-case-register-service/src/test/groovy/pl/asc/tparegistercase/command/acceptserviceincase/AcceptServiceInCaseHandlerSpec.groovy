package pl.asc.tparegistercase.command.acceptserviceincase

import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.domain.CaseService
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.CostReportServiceImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class AcceptServiceInCaseHandlerSpec extends Specification {

    def "should accept 1 service with qt 1"() {
        given:
            def repository = new CaseRepositoryTestImpl()
            def costReportServiceImpl = new CostReportServiceImpl()
            def mspPriceServiceTestImpl = new MspPriceServiceTestImpl()
            def service = new CaseService(repository, costReportServiceImpl, mspPriceServiceTestImpl)
            def create = service.create(new RegisterCaseCommand('93010267675', 'Jan', 'Kowalski', 'POL1', 'Warsaw'))
        when:
            new AcceptServiceInCaseHandler(service).handle(new AcceptServiceInCaseCommand(
                    caseNumber: create.caseNumber,
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 2,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

        then:
            def aCase = repository.findByCaseNumber(create.caseNumber)
            aCase.caseNumber.startsWith('CASE_')
            aCase.caseNumber.length() == 19
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(20)
            aCase.getInsured() != null
            aCase.costReport != null
    }
}
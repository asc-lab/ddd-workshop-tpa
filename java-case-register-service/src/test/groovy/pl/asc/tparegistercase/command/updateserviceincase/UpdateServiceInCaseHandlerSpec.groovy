package pl.asc.tparegistercase.command.updateserviceincase


import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.domain.CaseService
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
            def service = new CaseService(repository, costReportServiceImpl, mspPriceServiceTestImpl)
            def create = service.create(new RegisterCaseCommand('93010267675', 'Jan', 'Kowalski', 'POL1', 'Warsaw'))
        when:
            new AcceptServiceInCaseHandler(service).handle(new AcceptServiceInCaseCommand(
                    caseNumber: create.caseNumber,
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 2,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))


            new UpdateServiceInCaseHandler(service).handle(new UpdateServiceInCaseCommand(
                    caseNumber: create.caseNumber,
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now(),
                    orderNumber: 1))
        then:
            def aCase = repository.findByCaseNumber(create.caseNumber)
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(10)

    }
}
package pl.asc.tparegistercase.command.acceptserviceincase

import pl.asc.tparegistercase.CaseBuilder
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseHandler
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
            def aCase = repository.save(CaseBuilder.createCase('93010267675', 'Jan', 'Kowalski', 'POL1', 'Warsaw'))
        when:
            new AcceptServiceInCaseHandler(repository, costReportServiceImpl, mspPriceServiceTestImpl).handle(new AcceptServiceInCaseCommand(
                    caseNumber: aCase.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 2,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

        then:
            aCase.caseNumber.startsWith('CASE_')
            aCase.caseNumber.length() == 19
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(20)
    }



    def "should update service in case with order number 2"() {

    }

    def "should register case with 1 service, withe price and cost report"() {

        // dodać value object , opioisująćy cost report na casie, zwalidować go
        //1 * wywyłanie licznie cost repost
    }


    def ""() {

    }
}
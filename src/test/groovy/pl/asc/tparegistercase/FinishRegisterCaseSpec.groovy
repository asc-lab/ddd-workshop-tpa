package pl.asc.tparegistercase

import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCaseCommand
import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCaseHandler
import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCommandResult
import pl.asc.tparegistercase.command.addservice.AddServiceInCaseCommand
import pl.asc.tparegistercase.command.addservice.AddServiceInCaseCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseResult
import pl.asc.tparegistercase.command.rejectservice.RejectServiceInCaseCommand
import pl.asc.tparegistercase.command.rejectservice.RejectServiceInCaseHandler
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class FinishRegisterCaseSpec extends Specification {


    def "should finish registering with 1 service of quantity 2"() {

        given:
            def repository = new CaseRepositoryTestImpl()
            def mspPriceService = new MspPriceServiceTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
        when:
            RegisterCaseResult caseResult = new RegisterCaseCommandHandler(repository).handle(registerCaseCommand)

            new AddServiceInCaseCommandHandler(repository, mspPriceService).handle(new AddServiceInCaseCommand(
                    caseNumber: caseResult.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 2,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            EndRegistrationCommandResult acceptCaseResult = new EndRegistrationCaseHandler(repository).handle(new EndRegistrationCaseCommand(caseResult.caseNumber))
        then:
            def aCase = repository.findByCaseNumber(caseResult.caseNumber)

            caseResult.caseNumber.startsWith('CASE_')
            caseResult.caseNumber.length() == 19
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(20)
            acceptCaseResult.success
    }

    def "should finish registering case with 2 services and 1 rejected"() {
        given:
            def repository = new CaseRepositoryTestImpl()
            def mspPriceService = new MspPriceServiceTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
        when:
            RegisterCaseResult caseResult = new RegisterCaseCommandHandler(repository).handle(registerCaseCommand)

            new AddServiceInCaseCommandHandler(repository, mspPriceService).handle(new AddServiceInCaseCommand(
                    caseNumber: caseResult.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            new AddServiceInCaseCommandHandler(repository, mspPriceService).handle(new AddServiceInCaseCommand(
                    caseNumber: caseResult.getCaseNumber(),
                    serviceCode: 'DENTYSTA',
                    serviceQuantity: 1,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            // order przychodzi z UI
            new RejectServiceInCaseHandler(repository).handle(new RejectServiceInCaseCommand(caseResult.getCaseNumber(), 2, "To Expensive"))

            EndRegistrationCommandResult acceptCaseResult = new EndRegistrationCaseHandler(repository).handle(new EndRegistrationCaseCommand(caseResult.caseNumber))
        then:
            def aCase = repository.findByCaseNumber(caseResult.caseNumber)
            aCase.rejectedServices.size() == 1
            aCase.services.size() == 1
            aCase.totalPrice() == BigDecimal.valueOf(10)
            acceptCaseResult.success
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
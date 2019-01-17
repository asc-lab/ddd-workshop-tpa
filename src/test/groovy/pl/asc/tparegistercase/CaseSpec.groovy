package pl.asc.tparegistercase

import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCaseCommand
import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCaseHandler
import pl.asc.tparegistercase.command.acceptcase.EndRegistrationCommandResult
import pl.asc.tparegistercase.command.addservice.AddServiceCommand
import pl.asc.tparegistercase.command.addservice.AddServiceCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseResult
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class CaseSpec extends Specification {


    def "should register case with 1 service"() {

        given:
            def repository = new CaseRepositoryTestImpl()
            def mspPriceService = new MspPriceServiceTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
        when:
            RegisterCaseResult caseResult = new RegisterCaseCommandHandler(repository).handle(registerCaseCommand)

            new AddServiceCommandHandler(repository, mspPriceService).handle(new AddServiceCommand(
                    caseNumber: caseResult.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1L,
                    facilityCode: 'LUXMED_JEROZOLIMSKIE',
                    visitDate: LocalDateTime.now()))

            EndRegistrationCommandResult acceptCaseResult = new EndRegistrationCaseHandler(repository).handle(new EndRegistrationCaseCommand(caseResult.caseNumber))
        then:
            repository.findByCaseNumber(caseResult.caseNumber).totalPrice() == BigDecimal.valueOf(10)
            acceptCaseResult.success
    }

    def "should generate case number"() {
        given:
            def repository = new CaseRepositoryTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
        when:
            RegisterCaseResult caseResult = new RegisterCaseCommandHandler(repository).handle(registerCaseCommand)
        then:
            caseResult.caseNumber.startsWith('CASE_')
            caseResult.caseNumber.length() == 19
    }

    def "should register case with 2 services when 1 is rejected"() {

    }
}
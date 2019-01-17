package pl.asc.tparegistercase

import pl.asc.tparegistercase.command.acceptcase.AcceptCaseCommand
import pl.asc.tparegistercase.command.acceptcase.AcceptCaseCommandHandler
import pl.asc.tparegistercase.command.acceptcase.AcceptCaseResult
import pl.asc.tparegistercase.command.addservice.AddServiceCommand
import pl.asc.tparegistercase.command.addservice.AddServiceCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommandHandler
import pl.asc.tparegistercase.command.registercase.RegisterCaseResult
import pl.asc.tparegistercase.domain.CaseRepository
import spock.lang.Specification

import java.time.LocalDateTime

class CaseSpec extends Specification {

    def repository = Mock(CaseRepository)

    def "should register case with 1 service"() {

        given:
            def policyNr = 'pol1'
            def pesel = '93010267675'
            def visitDate = LocalDateTime.now()
            def firstName = 'Jan'
            def lastName = 'Kowalski'
            def city = 'Warsaw'
            def registerCaseCommand = new RegisterCaseCommand(pesel, firstName, lastName, policyNr, city)
        when:
            RegisterCaseResult caseResult = new RegisterCaseCommandHandler(repository).handle(registerCaseCommand)

            new AddServiceCommandHandler().handle(new AddServiceCommand(
                    caseNumber: caseResult.getCaseNumber(),
                    serviceCode: 'INTERNISTA',
                    serviceQuantity: 1L,
                    facilityCode: 'LUXMED_WARSZAWA_MORDOR',
                    visitDate: visitDate))


            AcceptCaseResult acceptCaseResult = new AcceptCaseCommandHandler().handle(new AcceptCaseCommand(caseResult.caseNumber))
        then:
            acceptCaseResult.success
    }

    def "should generate case number"() {

    }
}
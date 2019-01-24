package pl.asc.tparegistercase.command.registercase

import pl.asc.tparegistercase.domain.CaseService
import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import spock.lang.Specification

class RegisterCaseHandlerSpec extends Specification {


    def "should register case"() {

        given:
            def repository = new CaseRepositoryTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
            def service = new CaseService(repository, null, null)
        when:
            def caseResult = new RegisterCaseHandler(service).handle(registerCaseCommand)
        then:
            def aCase = repository.findByCaseNumber(caseResult.caseNumber)
            aCase.caseNumber.startsWith('CASE_')
            aCase.caseNumber.length() == 19
            aCase.services.size() == 0
            aCase.totalPrice() == BigDecimal.valueOf(0)
    }
}
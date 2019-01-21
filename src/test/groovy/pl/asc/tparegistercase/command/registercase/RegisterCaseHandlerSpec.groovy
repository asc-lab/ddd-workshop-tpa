package pl.asc.tparegistercase.command.registercase


import pl.asc.tparegistercase.infrastructure.CaseRepositoryTestImpl
import pl.asc.tparegistercase.infrastructure.CostReportServiceImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

class RegisterCaseHandlerSpec extends Specification {


    def "should register case"() {

        given:
            def repository = new CaseRepositoryTestImpl()
            def registerCaseCommand = new RegisterCaseCommand(pesel: '93010267675', firstName: 'Jan', lastName: 'Kowalski', policyNr: 'POL1', city: 'Warsaw')
        when:
            RegisterCaseResult caseResult = new RegisterCaseHandler(repository).handle(registerCaseCommand)
        then:
            def aCase = repository.findByCaseNumber(caseResult.caseNumber, new CostReportServiceImpl(), new MspPriceServiceTestImpl())
            aCase.caseNumber.startsWith('CASE_')
            aCase.caseNumber.length() == 19
            aCase.services.size() == 0
            aCase.totalPrice() == BigDecimal.valueOf(0)
    }
}
package pl.asc.tparegistercase

import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.domain.CaseFactory
import pl.asc.tparegistercase.domain.vo.Monetary
import spock.lang.Specification

import java.time.LocalDateTime

class CaseSpec extends Specification {

    def "should reorder service order numbers after rejection 1 service"() {
        given:
            def aCase = new CaseFactory(new RegisterCaseCommand()).create()
            aCase.addService("INTERNISTA1", 1, "F1", LocalDateTime.now(), Monetary.of("PLN", BigDecimal.TEN))
            aCase.addService("INTERNISTA2", 1, "F1", LocalDateTime.now(), Monetary.of("PLN", BigDecimal.TEN))
            aCase.addService("INTERNISTA3", 5, "F1", LocalDateTime.now(), Monetary.of("PLN", BigDecimal.TEN))
            aCase.addService("INTERNISTA4", 5, "F1", LocalDateTime.now(), Monetary.of("PLN", BigDecimal.TEN))
        when:
            aCase.rejectServiceInCase(3, "To Expensive.")
        then:
            aCase.services.get(0).order == 1
            aCase.services.get(1).order == 2
            aCase.services.get(2).order == 3
            aCase.services.get(3).order == 4
    }
}

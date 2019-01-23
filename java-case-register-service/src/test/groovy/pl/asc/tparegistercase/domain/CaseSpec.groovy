package pl.asc.tparegistercase.domain

import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.domain.CaseFactory
import pl.asc.tparegistercase.infrastructure.CostReportServiceImpl
import pl.asc.tparegistercase.infrastructure.MspPriceServiceTestImpl
import spock.lang.Specification

import java.time.LocalDateTime

class CaseSpec extends Specification {

    def "should reorder service order numbers after rejection 1 service"() {
        given:
            def aCase = new CaseFactory(new RegisterCaseCommand()).create()
            aCase.setMspPriceService(new MspPriceServiceTestImpl())
            aCase.setCostReportService(new CostReportServiceImpl())
            aCase.addService("INTERNISTA", 1, "MEDI_KRAKOW", LocalDateTime.now())
            aCase.addService("DENTYSTA", 3, "MEDI_KRAKOW", LocalDateTime.now())
            aCase.addService("INTERNISTA", 5, "MEDI_KRAKOW", LocalDateTime.now())
            aCase.addService("DENTYSTA", 5, "MEDI_KRAKOW", LocalDateTime.now())
        when:
            aCase.rejectServiceInCase(3, "To Expensive.")
        then:
            aCase.services.get(0).order == 1
            aCase.services.get(1).order == 2
            aCase.services.get(2).order == 3
    }

    def "should calculate total price"() {
        given:
            def aCase = new CaseFactory(new RegisterCaseCommand()).create()
            aCase.setMspPriceService(new MspPriceServiceTestImpl())
            aCase.setCostReportService(new CostReportServiceImpl())
            aCase.addService("INTERNISTA", 1, "LUXMED_JEROZOLIMSKIE", LocalDateTime.now())
            aCase.addService("DENTYSTA", 5, "LUXMED_JEROZOLIMSKIE", LocalDateTime.now())
        when:
            def totalPrice = aCase.totalPrice()
        then:
            totalPrice == BigDecimal.valueOf(60)
    }
}

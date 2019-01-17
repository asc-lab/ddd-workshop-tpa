package pl.asc.tparegistercase

import spock.lang.Specification


class TpaRegisterCaseApplicationSpec extends Specification {

    def "test"() {
        given:
            int a = 1
        when:
            a = a + 2
        then:
            a == 3
    }
}
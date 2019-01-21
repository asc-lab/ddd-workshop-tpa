package pl.asc.tparegistercase

import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand
import pl.asc.tparegistercase.domain.Case
import pl.asc.tparegistercase.domain.CaseFactory

class CaseBuilder {
    static Case createCase(String pesel, String firstName, String lastName, String policyNr, String city) {
        new CaseFactory(new RegisterCaseCommand(pesel, firstName, lastName, policyNr, city)).create()
    }
}

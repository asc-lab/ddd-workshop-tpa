package pl.asc.tparegistercase.domain;

import lombok.Getter;

import java.util.HashSet;
import java.util.Set;

public class Case {

    Case(String caseNumber, Insured insured, Set<MedicalService> services) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = services;
        this.caseEvents = new HashSet<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }


    @Getter
    private String caseNumber;
    private Insured insured;
    private Set<MedicalService> services;
    private Set<CaseEvent> caseEvents;

}

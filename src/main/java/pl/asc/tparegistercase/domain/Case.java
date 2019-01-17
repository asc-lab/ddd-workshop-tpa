package pl.asc.tparegistercase.domain;

import lombok.Getter;

import java.util.ArrayList;
import java.util.List;
import java.util.Set;

public class Case {

    @Getter
    private String caseNumber;
    private Insured insured;
    private Set<MedicalService> services;
    private List<CaseEvent> caseEvents;


    Case(String caseNumber, Insured insured, Set<MedicalService> services) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = services;
        this.caseEvents = new ArrayList<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }

    public void accept() {
        this.caseEvents.add(new AcceptedCaseEvent());
    }
}

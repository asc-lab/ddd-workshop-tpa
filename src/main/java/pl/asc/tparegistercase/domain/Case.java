package pl.asc.tparegistercase.domain;

import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.Monetary;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

public class Case {

    @Getter
    private String caseNumber;
    private Insured insured;
    private List<ServiceInCase> services;
    private List<CaseEvent> caseEvents;


    Case(String caseNumber, Insured insured, ArrayList<ServiceInCase> services) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = services;
        this.caseEvents = new ArrayList<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }

    public void accept() {
        this.caseEvents.add(new AcceptedCaseEvent());
    }

    public void addService(String serviceCode, Long serviceQuantity, String facilityCode, LocalDateTime visitDate, Monetary price) {
        this.services.add(
                new ServiceInCase(
                        services.size()+1,
                        serviceCode,
                        serviceQuantity,
                        facilityCode,
                        visitDate,
                        price
                )
        );
        this.caseEvents.add(new AddedMedicalServiceEvent());
    }

    public BigDecimal totalPrice() {
        return services.stream().map(e -> e.getPrice().getAmount()).reduce(BigDecimal.ZERO, BigDecimal::add);
    }
}

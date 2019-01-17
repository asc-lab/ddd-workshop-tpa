package pl.asc.tparegistercase.domain;

import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.Monetary;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

@Getter
public class Case {

    private String caseNumber;
    private Insured insured;
    private List<ServiceInCase> services;
    private List<RejectedServiceInCase> rejectedServices;
    private List<CaseEvent> caseEvents;


    Case(String caseNumber, Insured insured) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = new ArrayList<>();
        this.rejectedServices = new ArrayList<>();
        this.caseEvents = new ArrayList<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }

    public void accept() {
        this.caseEvents.add(new AcceptedCaseEvent());
    }

    public void addService(String serviceCode, Integer serviceQuantity, String facilityCode, LocalDateTime visitDate, Monetary price) {
        this.services.add(
                new ServiceInCase(
                        services.size() + 1,
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
        return services.stream()
                .map(e -> e.getPrice().getAmount().multiply(BigDecimal.valueOf(e.getServiceQuantity())))
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }

    public void rejectServiceInCase(Integer serviceOrderNumber, String rejectionReason) { //TODO what if there is nothing to reject?
        services.stream()
                .filter( service -> service.getOrder().equals(serviceOrderNumber))
                .findFirst()
                .ifPresent(serviceInCase -> {
                    services.remove(serviceInCase);
                    rejectedServices.add(
                            new RejectedServiceInCase(
                                    serviceInCase.getServiceCode(),
                                    serviceInCase.getServiceQuantity(),
                                    serviceInCase.getFacilityCode(),
                                    serviceInCase.getVisitDate(),
                                    Monetary.of(serviceInCase.getPrice()),
                                    rejectionReason
                            )
                    );
                    this.caseEvents.add(new RejectedCaseEvent());
                });
//        reorder() todo
    }

}

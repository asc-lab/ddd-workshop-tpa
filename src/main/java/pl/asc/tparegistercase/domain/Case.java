package pl.asc.tparegistercase.domain;

import lombok.Getter;
import lombok.Setter;
import pl.asc.tparegistercase.domain.vo.CostReport;
import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.IntStream;

@Getter
public class Case {

    private String caseNumber;
    private Insured insured;
    private List<ServiceInCase> services;
    private List<RejectedServiceInCase> rejectedServices;
    private List<CaseEvent> caseEvents;
    private List<CostReport> costReport;
    //transient
    @Setter
    private CostReportService costReportService;
    @Setter
    private MSPPriceService mspPriceService;

    Case(String caseNumber, Insured insured) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = new ArrayList<>();
        this.rejectedServices = new ArrayList<>();
        this.caseEvents = new ArrayList<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }

    public void finishCaseRegistration() {
        this.caseEvents.add(new CaseRegistrationFinishedEvent());
    }


    public void addService(String serviceCode, Integer serviceQuantity, String facilityCode, LocalDateTime visitDate) {
        if (mspPriceService == null) {
            throw new RuntimeException("MSPPriceService cannot be null.");
        }

        ServiceInCasePrice price = mspPriceService.findByFacilityAndService(facilityCode, serviceCode)
                .orElseThrow(() -> new RuntimeException("Price cannot be null."));

        this.services.add(
                new ServiceInCase(
                        services.size() + 1,
                        serviceCode,
                        serviceQuantity,
                        facilityCode,
                        visitDate,
                        price.getPrice()
                )
        );

        this.caseEvents.add(new ServiceInCaseAddedEvent());
        calculateCostReport();
    }

    public BigDecimal totalPrice() {
        return new ServiceInCaseCollection(this.services).totalPrice();
    }

    public void rejectServiceInCase(Integer serviceOrderNumber, String rejectionReason) {
        ServiceInCase serviceToReject = new ServiceInCaseCollection(this.services)
                .findByOrderNumber(serviceOrderNumber)
                .orElseThrow(() -> new RuntimeException("There is no service in case with number: " + serviceOrderNumber));

        this.services.remove(serviceToReject);
        this.rejectedServices.add(RejectedServiceInCase.fromExistingServiceInCase(serviceToReject, rejectionReason));
        this.caseEvents.add(new RejectedCaseEvent());
        reNumberServicesInCase();
        calculateCostReport();
    }

    private void calculateCostReport() {
        if (costReportService == null) {
            throw new RuntimeException("CostReportService cannot be null.");
        }
        this.costReport = costReportService.calculate(this.services);
    }

    private void reNumberServicesInCase() {
        IntStream.range(0, this.services.size()).forEach(i -> this.services.get(i).reNumber(++i));
    }


}

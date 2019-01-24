package pl.asc.tparegistercase.domain;

import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.CostReport;
import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.IntStream;

@Getter
public class Case {

    private String id;
    private String caseNumber;
    private Insured insured;
    private List<ServiceInCase> services;
    private List<RejectedServiceInCase> rejectedServices;
    private List<CostReport> costReport;
    private List<CaseEvent> caseEvents;

    Case(String caseNumber, Insured insured) {
        this.caseNumber = caseNumber;
        this.insured = insured;
        this.services = new ArrayList<>();
        this.rejectedServices = new ArrayList<>();
        this.caseEvents = new ArrayList<>();
        this.caseEvents.add(new RegisteredCaseEvent());
    }

    void finishCaseRegistration() {
        this.caseEvents.add(new CaseRegistrationFinishedEvent());
    }

    void addService(String serviceCode,
                           Integer serviceQuantity,
                           String facilityCode,
                           LocalDateTime visitDate,
                           CostReportService costReportService,
                           MSPPriceService mspPriceService) {
        this.services.add(
                new ServiceInCase(
                        services.size() + 1,
                        serviceCode,
                        serviceQuantity,
                        facilityCode,
                        visitDate,
                        calculateServiceInCasePrice(mspPriceService, serviceCode, facilityCode).getPrice()
                )
        );

        this.caseEvents.add(new ServiceInCaseAddedEvent());
        calculateCostReport(costReportService);
    }


    void updateService(String serviceCode,
                       Integer serviceQuantity,
                       String facilityCode,
                       LocalDateTime visitDate,
                       Integer orderNumber,
                       CostReportService costReportService,
                       MSPPriceService mspPriceService) {
        ServiceInCase serviceInCase = serviceInCaseCollection()
                .findByOrderNumber(orderNumber)
                .orElseThrow(() -> new RuntimeException("There is no service in case with number: "));

        serviceInCase.update(serviceCode,
                serviceQuantity,
                facilityCode,
                visitDate,
                calculateServiceInCasePrice(mspPriceService, serviceCode, facilityCode).getPrice());
        this.caseEvents.add(new ServiceInCaseUpdatedEvent());
        calculateCostReport(costReportService);
    }

    void rejectServiceInCase(Integer serviceOrderNumber,
                             String rejectionReason,
                             CostReportService costReportService) {
        ServiceInCase serviceToReject = serviceInCaseCollection()
                .findByOrderNumber(serviceOrderNumber)
                .orElseThrow(() -> new RuntimeException("There is no service in case with number: " + serviceOrderNumber));

        this.services.remove(serviceToReject);
        this.rejectedServices.add(RejectedServiceInCase.fromExistingServiceInCase(serviceToReject, rejectionReason));
        this.caseEvents.add(new RejectedCaseEvent());
        reNumberServicesInCase();
        calculateCostReport(costReportService);
    }

    public BigDecimal totalPrice() {
        return serviceInCaseCollection().totalPrice();
    }

    private ServiceInCasePrice calculateServiceInCasePrice(MSPPriceService mspPriceService, String serviceCode, String facilityCode) {
        if (mspPriceService == null) {
            throw new RuntimeException("MSPPriceService cannot be null.");
        }
        return mspPriceService.findByFacilityAndService(serviceCode, facilityCode)
                .orElseThrow(() -> new RuntimeException("Price cannot be null."));
    }

    private void calculateCostReport(CostReportService costReportService) {
        if (costReportService == null) {
            throw new RuntimeException("CostReportService cannot be null.");
        }
        this.costReport = costReportService.calculate(this.services);
    }

    private void reNumberServicesInCase() {
        IntStream.range(0, this.services.size()).forEach(i -> this.services.get(i).reNumber(++i));
    }

    private ServiceInCaseCollection serviceInCaseCollection() {
        return new ServiceInCaseCollection(this.services);
    }


}

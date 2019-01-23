package pl.asc.tparegistercase.domain;

public interface CaseRepository {

    Case save(Case registeredCase);

    Case findByCaseNumber(String caseNumber, CostReportService costReportService, MSPPriceService mspPriceService);
}

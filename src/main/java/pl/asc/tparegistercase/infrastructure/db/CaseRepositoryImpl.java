package pl.asc.tparegistercase.infrastructure.db;

import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.MSPPriceService;

public class CaseRepositoryImpl implements CaseRepository {
    @Override
    public Case save(Case registeredCase) {
        return null;
    }

    @Override
    public Case findByCaseNumber(String caseNumber, CostReportService costReportService, MSPPriceService mspPriceService) {
        return null;
    }
}

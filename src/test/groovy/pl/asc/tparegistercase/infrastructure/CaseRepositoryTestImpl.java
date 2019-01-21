package pl.asc.tparegistercase.infrastructure;

import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.MSPPriceService;

import java.util.HashSet;
import java.util.Set;

public class CaseRepositoryTestImpl implements CaseRepository {

    private Set<Case> cases;

    public CaseRepositoryTestImpl() {
        this.cases = new HashSet<>();
    }

    @Override
    public Case save(Case registeredCase) {
        this.cases.add(registeredCase);
        return registeredCase;
    }

    @Override
    public Case findByCaseNumber(String caseNumber, CostReportService costReportService, MSPPriceService mspPriceService) {
        return cases.stream()
                .filter(e -> e.getCaseNumber().equals(caseNumber))
                .peek(aCase -> {
                    aCase.setCostReportService(costReportService);
                    aCase.setMspPriceService(mspPriceService);
                })
                .findFirst()
                .orElseThrow(() -> new RuntimeException("Case not found."));
    }
}

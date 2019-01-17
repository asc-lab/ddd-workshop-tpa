package pl.asc.tparegistercase.infrastructure;

import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;

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
    public Case findByCaseNumber(String caseNumber) {
        return cases.stream()
                .filter(e -> e.getCaseNumber().equals(caseNumber))
                .findFirst()
                .orElseThrow(() -> new RuntimeException("Case not found."));
    }
}

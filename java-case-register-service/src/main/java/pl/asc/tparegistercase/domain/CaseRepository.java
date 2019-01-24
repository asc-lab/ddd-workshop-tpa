package pl.asc.tparegistercase.domain;

public interface CaseRepository {

    void save(Case registeredCase);

    Case findByCaseNumber(String caseNumber);
}

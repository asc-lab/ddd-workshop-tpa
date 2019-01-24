package pl.asc.tparegistercase.infrastructure.db;

import lombok.RequiredArgsConstructor;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.MSPPriceService;

@Repository
@RequiredArgsConstructor
public class CaseRepositoryImpl implements CaseRepository {

    private final JdbcTemplate jdbcTemplate;

    @Override
    public void save(Case registeredCase) {
    }

    @Override
    public Case findByCaseNumber(String caseNumber) {
        return null;
    }
}

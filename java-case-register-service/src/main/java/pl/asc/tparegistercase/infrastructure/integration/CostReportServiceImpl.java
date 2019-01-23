package pl.asc.tparegistercase.infrastructure.integration;

import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.ServiceInCase;
import pl.asc.tparegistercase.domain.vo.CostReport;

import java.util.List;

public class CostReportServiceImpl implements CostReportService {
    @Override
    public List<CostReport> calculate(List<ServiceInCase> services) {
        return null;
    }
}

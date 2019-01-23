package pl.asc.tparegistercase.domain;

import pl.asc.tparegistercase.domain.vo.CostReport;

import java.util.List;

public interface CostReportService {
    List<CostReport> calculate(List<ServiceInCase> services);
}

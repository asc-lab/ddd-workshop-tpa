package pl.asc.tparegistercase.infrastructure;

import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.ServiceInCase;
import pl.asc.tparegistercase.domain.vo.CostReport;

import java.util.ArrayList;
import java.util.List;

public class CostReportServiceImpl implements CostReportService {
    @Override
    public List<CostReport> calculate(List<ServiceInCase> services) { // przez to ServiceInCase jest publiczne
        return new ArrayList<>(); //fixme integration
    }
}

package pl.asc.tparegistercase.command.acceptserviceincase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.MSPPriceService;

@RequiredArgsConstructor
public class AcceptServiceInCaseHandler implements CommandHandler<AcceptServiceInCaseResult, AcceptServiceInCaseCommand> {

    private final CaseRepository caseRepository;
    private final CostReportService costReportService;
    private final MSPPriceService mspPriceService;

    @Override
    public AcceptServiceInCaseResult handle(AcceptServiceInCaseCommand cmd) {
        Case aCase = caseRepository.findByCaseNumber(cmd.getCaseNumber(), costReportService, mspPriceService);
        aCase.addService(cmd.getServiceCode(), cmd.getServiceQuantity(), cmd.getFacilityCode(), cmd.getVisitDate());
        caseRepository.save(aCase);
        return new AcceptServiceInCaseResult();
    }
}

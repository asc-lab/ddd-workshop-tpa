package pl.asc.tparegistercase.command.rejectservice;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;

@RequiredArgsConstructor
public class RejectServiceInCaseHandler implements CommandHandler<RejectServiceInCaseResult, RejectServiceInCaseCommand> {

    private final CaseRepository caseRepository;
    private final CostReportService costReportService;

    @Override
    public RejectServiceInCaseResult handle(RejectServiceInCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber(), costReportService, null);
        aCase.rejectServiceInCase(command.getServiceOrderNumber(), command.getRejectionReason());
        caseRepository.save(aCase);
        return new RejectServiceInCaseResult();
    }
}

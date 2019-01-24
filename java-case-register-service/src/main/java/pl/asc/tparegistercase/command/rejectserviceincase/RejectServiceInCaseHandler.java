package pl.asc.tparegistercase.command.rejectserviceincase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CaseService;
import pl.asc.tparegistercase.domain.CostReportService;

@RequiredArgsConstructor
public class RejectServiceInCaseHandler implements CommandHandler<RejectServiceInCaseResult, RejectServiceInCaseCommand> {

    private final CaseService caseService;

    @Override
    public RejectServiceInCaseResult handle(RejectServiceInCaseCommand command) {
        return caseService.rejectServiceInCase(command);

    }
}

package pl.asc.tparegistercase.command.rejectservice;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;

@RequiredArgsConstructor
public class RejectServiceInCaseHandler implements CommandHandler<RejectServiceInCaseResult, RejectServiceInCaseCommand> {

    private final CaseRepository caseRepository;

    @Override
    public RejectServiceInCaseResult handle(RejectServiceInCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        aCase.rejectServiceInCase(command.getServiceOrderNumber(), command.getRejectionReason());
        //todo cost report service
        return new RejectServiceInCaseResult();
    }
}

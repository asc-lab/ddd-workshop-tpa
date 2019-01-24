package pl.asc.tparegistercase.command.acceptserviceincase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.CaseService;

@RequiredArgsConstructor
public class AcceptServiceInCaseHandler implements CommandHandler<AcceptServiceInCaseResult, AcceptServiceInCaseCommand> {

    private final CaseService caseService;

    @Override
    public AcceptServiceInCaseResult handle(AcceptServiceInCaseCommand cmd) {
        return caseService.addService(cmd);
    }
}

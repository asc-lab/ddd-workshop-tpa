package pl.asc.tparegistercase.command.updateserviceincase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.CaseService;

@RequiredArgsConstructor

public class UpdateServiceInCaseHandler implements CommandHandler<UpdateServiceInCaseResult, UpdateServiceInCaseCommand> {

    private final CaseService caseService;

    @Override
    public UpdateServiceInCaseResult handle(UpdateServiceInCaseCommand command) {
        return caseService.updateService(command);
    }
}

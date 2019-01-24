package pl.asc.tparegistercase.command.finishcaseregistration;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.CaseService;

@RequiredArgsConstructor
public class FinishCaseRegistrationHandler implements CommandHandler<FinishCaseRegistrationResult, FinishCaseRegistrationCommand> {

    private final CaseService caseService;

    @Override
    public FinishCaseRegistrationResult handle(FinishCaseRegistrationCommand command) {
        return caseService.finishCaseRegistration(command.getCaseNumber());
    }
}

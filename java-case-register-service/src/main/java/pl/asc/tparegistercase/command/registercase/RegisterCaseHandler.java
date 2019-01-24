package pl.asc.tparegistercase.command.registercase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseFactory;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CaseService;

@RequiredArgsConstructor
public class RegisterCaseHandler implements CommandHandler<RegisterCaseResult, RegisterCaseCommand> {

    private final CaseService caseService;

    @Override
    public RegisterCaseResult handle(RegisterCaseCommand command) {
        return caseService.create(command);
    }
}

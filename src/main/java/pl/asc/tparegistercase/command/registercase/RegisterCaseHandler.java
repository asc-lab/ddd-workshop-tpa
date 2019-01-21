package pl.asc.tparegistercase.command.registercase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseFactory;
import pl.asc.tparegistercase.domain.CaseRepository;

@RequiredArgsConstructor
public class RegisterCaseHandler implements CommandHandler<RegisterCaseResult, RegisterCaseCommand> {

    final CaseRepository repository;

    @Override
    public RegisterCaseResult handle(RegisterCaseCommand command) {
        Case registeredCase = new CaseFactory(command).create();
        repository.save(registeredCase);
        return new RegisterCaseResult(registeredCase.getCaseNumber());
    }
}

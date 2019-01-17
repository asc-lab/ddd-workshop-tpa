package pl.asc.tparegistercase.command.acceptcase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.specification.NotNullSpecification;

@RequiredArgsConstructor
public class EndRegistrationCaseHandler implements CommandHandler<EndRegistrationCommandResult, EndRegistrationCaseCommand> {

    private final CaseRepository caseRepository;

    @Override
    public EndRegistrationCommandResult handle(EndRegistrationCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        new NotNullSpecification("case not null").ensureIsSatisfiedBy(aCase);
        aCase.accept();
        caseRepository.save(aCase);
        return new EndRegistrationCommandResult(true);
    }
}

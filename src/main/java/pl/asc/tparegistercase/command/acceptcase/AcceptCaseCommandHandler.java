package pl.asc.tparegistercase.command.acceptcase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;

@RequiredArgsConstructor
public class AcceptCaseCommandHandler implements CommandHandler<AcceptCaseResult, AcceptCaseCommand> {

    private final CaseRepository caseRepository;

    @Override
    public AcceptCaseResult handle(AcceptCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        //todo exception when not found
        aCase.accept();
        caseRepository.save(aCase);
        return new AcceptCaseResult(true);
    }
}

package pl.asc.tparegistercase.command.finishcaseregistration;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;

@RequiredArgsConstructor
public class FinishCaseRegistrationHandler implements CommandHandler<FinishCaseRegistrationResult, FinishCaseRegistrationCommand> {

    private final CaseRepository caseRepository;

    @Override
    public FinishCaseRegistrationResult handle(FinishCaseRegistrationCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber(), null, null);
        aCase.finishCaseRegistration();
        caseRepository.save(aCase);
        return new FinishCaseRegistrationResult(true);
    }
}

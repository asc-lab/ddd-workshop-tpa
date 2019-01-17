package pl.asc.tparegistercase.command.addservice;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;

@RequiredArgsConstructor
public class AddServiceCommandHandler implements CommandHandler<AddServiceResult, AddServiceCommand> {

    private final CaseRepository caseRepository;

    @Override
    public AddServiceResult handle(AddServiceCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        // FIXME validation and exceptions handling
        aCase.addService(
                command.getServiceCode(),
                command.getServiceQuantity(),
                command.getFacilityCode(),
                command.getVisitDate()
        );
        caseRepository.save(aCase);
        return new AddServiceResult();
    }
}

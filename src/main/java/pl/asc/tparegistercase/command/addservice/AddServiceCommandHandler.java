package pl.asc.tparegistercase.command.addservice;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.MSPPriceService;
import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;
import pl.asc.tparegistercase.specification.NotNullSpecification;

@RequiredArgsConstructor
public class AddServiceCommandHandler implements CommandHandler<AddServiceResult, AddServiceCommand> {

    private final CaseRepository caseRepository;
    private final MSPPriceService mspPriceService;

    @Override
    public AddServiceResult handle(AddServiceCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        ServiceInCasePrice serviceInCasePrice = mspPriceService
                .findByFacilityAndService(command.getFacilityCode(), command.getServiceCode());
        new NotNullSpecification("Case not found.").ensureIsSatisfiedBy(aCase);
        new NotNullSpecification("Price not found.").ensureIsSatisfiedBy(serviceInCasePrice);
        aCase.addService(
                command.getServiceCode(),
                command.getServiceQuantity(),
                command.getFacilityCode(),
                command.getVisitDate(),
                serviceInCasePrice.getPrice()
        );
        caseRepository.save(aCase);
        return new AddServiceResult();
    }
}

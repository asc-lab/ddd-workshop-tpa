package pl.asc.tparegistercase.command.updateserviceincase;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.cqs.CommandHandler;
import pl.asc.tparegistercase.domain.Case;
import pl.asc.tparegistercase.domain.CaseRepository;
import pl.asc.tparegistercase.domain.CostReportService;
import pl.asc.tparegistercase.domain.MSPPriceService;

@RequiredArgsConstructor

public class UpdateServiceInCaseHandler implements CommandHandler<UpdateServiceInCaseResult, UpdateServiceInCaseCommand> {

    private final CaseRepository caseRepository;
    private final CostReportService costReportService;
    private final MSPPriceService mspPriceService;

    @Override
    public UpdateServiceInCaseResult handle(UpdateServiceInCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber(), costReportService, mspPriceService);
        aCase.updateService(command.getServiceCode(),
                command.getServiceQuantity(),
                command.getFacilityCode(),
                command.getVisitDate(),
                command.getOrderNumber());
        caseRepository.save(aCase);
        return new UpdateServiceInCaseResult();
    }
}

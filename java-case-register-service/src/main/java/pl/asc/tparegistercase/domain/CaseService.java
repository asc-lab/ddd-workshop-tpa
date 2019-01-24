package pl.asc.tparegistercase.domain;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand;
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseResult;
import pl.asc.tparegistercase.command.finishcaseregistration.FinishCaseRegistrationResult;
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand;
import pl.asc.tparegistercase.command.registercase.RegisterCaseResult;
import pl.asc.tparegistercase.command.rejectserviceincase.RejectServiceInCaseCommand;
import pl.asc.tparegistercase.command.rejectserviceincase.RejectServiceInCaseResult;
import pl.asc.tparegistercase.command.updateserviceincase.UpdateServiceInCaseCommand;
import pl.asc.tparegistercase.command.updateserviceincase.UpdateServiceInCaseResult;

@Service
@RequiredArgsConstructor
public class CaseService {

    private final CaseRepository caseRepository;
    private final CostReportService costReportService;
    private final MSPPriceService mspPriceService;

    public AcceptServiceInCaseResult addService(AcceptServiceInCaseCommand cmd) {
        Case aCase = caseRepository.findByCaseNumber(cmd.getCaseNumber());
        aCase.addService(cmd.getServiceCode(),
                cmd.getServiceQuantity(),
                cmd.getFacilityCode(),
                cmd.getVisitDate(),
                costReportService,
                mspPriceService);
        caseRepository.save(aCase);
        return new AcceptServiceInCaseResult();
    }

    public FinishCaseRegistrationResult finishCaseRegistration(String caseNumber) {
        Case aCase = caseRepository.findByCaseNumber(caseNumber);
        aCase.finishCaseRegistration();
        caseRepository.save(aCase);
        return new FinishCaseRegistrationResult(true);
    }

    public RegisterCaseResult create(RegisterCaseCommand command) {
        Case registeredCase = new CaseFactory(command).create();
        caseRepository.save(registeredCase);
        return new RegisterCaseResult(registeredCase.getCaseNumber());
    }

    public RejectServiceInCaseResult rejectServiceInCase(RejectServiceInCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        aCase.rejectServiceInCase(command.getServiceOrderNumber(), command.getRejectionReason(), costReportService);
        caseRepository.save(aCase);
        return new RejectServiceInCaseResult();
    }

    public UpdateServiceInCaseResult updateService(UpdateServiceInCaseCommand command) {
        Case aCase = caseRepository.findByCaseNumber(command.getCaseNumber());
        aCase.updateService(command.getServiceCode(),
                command.getServiceQuantity(),
                command.getFacilityCode(),
                command.getVisitDate(),
                command.getOrderNumber(),
                costReportService,
                mspPriceService);
        caseRepository.save(aCase);
        return new UpdateServiceInCaseResult();
    }
}
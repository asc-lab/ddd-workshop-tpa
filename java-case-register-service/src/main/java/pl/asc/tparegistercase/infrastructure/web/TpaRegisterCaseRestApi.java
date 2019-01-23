package pl.asc.tparegistercase.infrastructure.web;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseCommand;
import pl.asc.tparegistercase.command.acceptserviceincase.AcceptServiceInCaseResult;
import pl.asc.tparegistercase.command.finishcaseregistration.FinishCaseRegistrationCommand;
import pl.asc.tparegistercase.command.finishcaseregistration.FinishCaseRegistrationResult;
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand;
import pl.asc.tparegistercase.command.registercase.RegisterCaseResult;
import pl.asc.tparegistercase.command.rejectserviceincase.RejectServiceInCaseCommand;
import pl.asc.tparegistercase.command.rejectserviceincase.RejectServiceInCaseResult;
import pl.asc.tparegistercase.command.updateserviceincase.UpdateServiceInCaseCommand;
import pl.asc.tparegistercase.command.updateserviceincase.UpdateServiceInCaseResult;
import pl.asc.tparegistercase.cqs.CommandBus;

@RequiredArgsConstructor
@RestController(value = "/api/tpa")
public class TpaRegisterCaseRestApi {

    private final CommandBus bus;

    @PostMapping("/acceptserviceincase")
    public AcceptServiceInCaseResult submit(@RequestBody AcceptServiceInCaseCommand cmd) {
        return bus.executeCommand(cmd);
    }

    @PostMapping("/finishcaseregistration")
    public FinishCaseRegistrationResult submit(@RequestBody FinishCaseRegistrationCommand cmd) {
        return bus.executeCommand(cmd);
    }

    @PostMapping("/registercase")
    public RegisterCaseResult submit(@RequestBody RegisterCaseCommand cmd) {
        return bus.executeCommand(cmd);
    }

    @PostMapping("/rejectserviceincase")
    public RejectServiceInCaseResult submit(@RequestBody RejectServiceInCaseCommand cmd) {
        return bus.executeCommand(cmd);
    }

    @PostMapping("/updateserviceincase")
    public UpdateServiceInCaseResult submit(@RequestBody UpdateServiceInCaseCommand cmd) {
        return bus.executeCommand(cmd);
    }

}

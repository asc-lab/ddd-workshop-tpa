package pl.asc.tparegistercase.infrastructure.web;

import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.RestController;
import pl.asc.tparegistercase.cqs.CommandBus;

@RequiredArgsConstructor
@RestController(value = "tpa")
public class TpaRegisterCaseController {

    private final CommandBus commandBus;
}

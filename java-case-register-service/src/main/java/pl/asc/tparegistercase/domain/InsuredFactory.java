package pl.asc.tparegistercase.domain;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand;

@RequiredArgsConstructor
class InsuredFactory {

    private final RegisterCaseCommand command;

    Insured create() {
        return new Insured(); //FIXME implement
    }
}

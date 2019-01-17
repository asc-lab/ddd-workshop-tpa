package pl.asc.tparegistercase.domain;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand;

import java.util.HashSet;
import java.util.UUID;

@RequiredArgsConstructor
public class CaseFactory {
    private final RegisterCaseCommand command;

    public Case create() {

        return new Case(
                new CaseNumberGenerator().generate(),
                new InsuredFactory(command).create(),
                new HashSet<>()

        );
    }

    private class CaseNumberGenerator {
        String generate(){
            return UUID.randomUUID().toString();
        }
    }
}

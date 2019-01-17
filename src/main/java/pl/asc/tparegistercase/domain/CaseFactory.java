package pl.asc.tparegistercase.domain;

import lombok.RequiredArgsConstructor;
import pl.asc.tparegistercase.command.registercase.RegisterCaseCommand;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;

@RequiredArgsConstructor
public class CaseFactory {
    private final RegisterCaseCommand command;

    public Case create() {
        return new Case(
                new CaseNumberGenerator().generate(),
                new InsuredFactory(command).create(),
                new ArrayList<>()

        );
    }

    private class CaseNumberGenerator {
        String generate() {
            DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyyMMddHHmmSS");
            String prefix = "CASE_";
            return prefix + LocalDateTime.now().format(formatter);
        }
    }
}

package pl.asc.tparegistercase.command.acceptcase;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import pl.asc.tparegistercase.cqs.Command;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class AcceptCaseCommand implements Command<AcceptCaseResult> {
    private String caseNumber;
}

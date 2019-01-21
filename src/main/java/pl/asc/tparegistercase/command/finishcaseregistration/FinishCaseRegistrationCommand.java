package pl.asc.tparegistercase.command.finishcaseregistration;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import pl.asc.tparegistercase.cqs.Command;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class FinishCaseRegistrationCommand implements Command<FinishCaseRegistrationResult> {
    private String caseNumber;
}

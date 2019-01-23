package pl.asc.tparegistercase.command.rejectserviceincase;

import lombok.AllArgsConstructor;
import lombok.Getter;
import pl.asc.tparegistercase.cqs.Command;

@AllArgsConstructor
@Getter
public class RejectServiceInCaseCommand implements Command<RejectServiceInCaseResult> {
    private String caseNumber;
    private Integer serviceOrderNumber;
    private String rejectionReason;
}

package pl.asc.tparegistercase.command.acceptserviceincase;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import pl.asc.tparegistercase.cqs.Command;

import java.time.LocalDateTime;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class AcceptServiceInCaseCommand implements Command<AcceptServiceInCaseResult> {
    private String caseNumber;
    private String serviceCode;
    private Integer serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
}

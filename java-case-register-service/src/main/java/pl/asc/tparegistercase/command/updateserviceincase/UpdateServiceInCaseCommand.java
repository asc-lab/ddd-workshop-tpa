package pl.asc.tparegistercase.command.updateserviceincase;

import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import pl.asc.tparegistercase.cqs.Command;

import java.time.LocalDateTime;

@Getter
@Setter
@NoArgsConstructor
public class UpdateServiceInCaseCommand implements Command<UpdateServiceInCaseResult> {
    private String caseNumber;
    private String serviceCode;
    private Integer serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
    private Integer orderNumber;
}

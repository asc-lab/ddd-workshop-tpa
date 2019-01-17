package pl.asc.tparegistercase.command.addservice;

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
public class AddServiceCommand implements Command<AddServiceResult> {
    private String caseNumber;
    private String serviceCode;
    private Long serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
}

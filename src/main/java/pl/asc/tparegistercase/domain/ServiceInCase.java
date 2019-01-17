package pl.asc.tparegistercase.domain;

import lombok.AllArgsConstructor;
import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.Monetary;

import java.time.LocalDateTime;

@AllArgsConstructor
@Getter
class ServiceInCase {

    private Integer order;
    private String serviceCode;
    private Long serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
    private Monetary price;

}

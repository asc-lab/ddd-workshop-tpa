package pl.asc.tparegistercase.domain;

import lombok.AllArgsConstructor;
import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.Monetary;

import java.time.LocalDateTime;

@AllArgsConstructor
@Getter
public class ServiceInCase {

    private Integer order;
    private String serviceCode;
    private Integer serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
    private Monetary price;

    void reNumber(int order) {
        this.order = order;
    }

    void update(String serviceCode, Integer serviceQuantity, String facilityCode, LocalDateTime visitDate, Monetary price) {
        this.serviceCode = serviceCode;
        this.serviceQuantity = serviceQuantity;
        this.facilityCode = facilityCode;
        this.visitDate = visitDate;
        this.price = price;
    }
}

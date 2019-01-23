package pl.asc.tparegistercase.domain.vo;

import lombok.AllArgsConstructor;
import lombok.Getter;

@AllArgsConstructor
@Getter
public class ServiceInCasePrice {
    private String facilityCode;
    private String serviceCode;
    private Monetary price;
}

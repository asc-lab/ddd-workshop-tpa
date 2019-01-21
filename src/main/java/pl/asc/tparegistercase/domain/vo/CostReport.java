package pl.asc.tparegistercase.domain.vo;

import lombok.AllArgsConstructor;
import lombok.Getter;

import java.math.BigDecimal;

@AllArgsConstructor
@Getter
public class CostReport {
    private BigDecimal TUCost;
    private BigDecimal insuredCost;
    private String explanation;
}

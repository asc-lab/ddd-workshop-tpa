package pl.asc.tparegistercase.domain.vo;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Getter;

import java.math.BigDecimal;

@Getter
@AllArgsConstructor(access = AccessLevel.PRIVATE)
public class Monetary {

    private String currency;
    private BigDecimal amount;

    public static Monetary zero(String currency) {
        return new Monetary(currency, BigDecimal.ZERO);
    }

    public static Monetary of(String currency, BigDecimal amount) {
        return new Monetary(currency, amount);
    }
}

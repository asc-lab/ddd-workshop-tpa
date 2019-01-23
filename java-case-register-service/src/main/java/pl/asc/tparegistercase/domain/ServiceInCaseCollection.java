package pl.asc.tparegistercase.domain;

import lombok.RequiredArgsConstructor;

import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;

@RequiredArgsConstructor
class ServiceInCaseCollection {

    private final List<ServiceInCase> services;

    BigDecimal totalPrice() {
        return services.stream()
                .map(e -> e.getPrice().getAmount().multiply(BigDecimal.valueOf(e.getServiceQuantity())))
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }

    Optional<ServiceInCase> findByOrderNumber(Integer serviceOrderNumber) {
        return this.services.stream().filter(serviceInCase -> serviceInCase.getOrder().equals(serviceOrderNumber)).findFirst();
    }
}

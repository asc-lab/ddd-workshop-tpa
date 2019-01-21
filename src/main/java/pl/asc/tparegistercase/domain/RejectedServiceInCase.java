package pl.asc.tparegistercase.domain;

import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.Getter;
import pl.asc.tparegistercase.domain.vo.Monetary;

import java.time.LocalDateTime;

@Getter
@AllArgsConstructor(access = AccessLevel.PRIVATE)
class RejectedServiceInCase {

    private String serviceCode;
    private Integer serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;
    private Monetary price;
    private String rejectionReason;

    static RejectedServiceInCase fromExistingServiceInCase(ServiceInCase serviceInCase, String rejectionReason) {
        if (serviceInCase == null)
            return null;
        return new RejectedServiceInCase(
                serviceInCase.getServiceCode(),
                serviceInCase.getServiceQuantity(),
                serviceInCase.getFacilityCode(),
                serviceInCase.getVisitDate(),
                Monetary.of(serviceInCase.getPrice()),
                rejectionReason
        );
    }
}

package pl.asc.tparegistercase.domain;

import lombok.AllArgsConstructor;

import java.time.LocalDateTime;

@AllArgsConstructor
class MedicalService {

    private String serviceCode;
    private Long serviceQuantity;
    private String facilityCode;
    private LocalDateTime visitDate;

}

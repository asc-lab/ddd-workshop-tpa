package pl.asc.tparegistercase.domain;

import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

import java.util.Optional;

public interface MSPPriceService {

    Optional<ServiceInCasePrice> findByFacilityAndService(String facilityCode, String serviceCode);

}

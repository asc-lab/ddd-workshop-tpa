package pl.asc.tparegistercase.domain;

import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

public interface MSPPriceService {

    ServiceInCasePrice findByFacilityAndService(String facilityCode, String serviceCode);

}

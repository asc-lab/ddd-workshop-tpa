package pl.asc.tparegistercase.infrastructure;

import pl.asc.tparegistercase.domain.MSPPriceService;
import pl.asc.tparegistercase.domain.vo.Monetary;
import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public class MspPriceServiceTestImpl implements MSPPriceService {

    private static List<ServiceInCasePrice> pricing = new ArrayList<>();

    static {
        pricing.add(new ServiceInCasePrice("LUXMED_JEROZOLIMSKIE", "INTERNISTA", Monetary.of("PLN", BigDecimal.TEN)));
        pricing.add(new ServiceInCasePrice("LUXMED_JEROZOLIMSKIE", "DENTYSTA", Monetary.of("PLN", BigDecimal.TEN)));
        pricing.add(new ServiceInCasePrice("MEDI_KRAKOW", "INTERNISTA", Monetary.of("PLN", BigDecimal.TEN)));
        pricing.add(new ServiceInCasePrice("MEDI_KRAKOW", "DENTYSTA", Monetary.of("PLN", BigDecimal.TEN)));
    }

    @Override
    public Optional<ServiceInCasePrice> findByFacilityAndService(String serviceCode, String facilityCode) {
        return pricing.stream()
                .filter(p -> p.getFacilityCode().equals(facilityCode))
                .filter(p -> p.getServiceCode().equals(serviceCode))
                .findFirst();
    }

}

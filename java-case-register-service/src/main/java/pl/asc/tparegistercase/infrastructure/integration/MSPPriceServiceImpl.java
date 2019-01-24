package pl.asc.tparegistercase.infrastructure.integration;

import org.springframework.stereotype.Service;
import pl.asc.tparegistercase.domain.MSPPriceService;
import pl.asc.tparegistercase.domain.vo.ServiceInCasePrice;

import java.util.Optional;

@Service
public class MSPPriceServiceImpl implements MSPPriceService {
    @Override
    public Optional<ServiceInCasePrice> findByFacilityAndService(String serviceCode, String facilityCode) {
        return Optional.empty();
    }
}

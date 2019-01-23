package pl.asc.tparegistercase;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.Bean;
import pl.asc.tparegistercase.cqs.CommandBus;
import pl.asc.tparegistercase.cqs.Registry;
import pl.asc.tparegistercase.cqs.SpringCommandBus;

@SpringBootApplication
public class TpaRegisterCaseApplication {

    public static void main(String[] args) {
        SpringApplication.run(TpaRegisterCaseApplication.class, args);
    }

    @Bean
    public Registry registry(ApplicationContext applicationContext) {
        return new Registry(applicationContext);
    }

    @Bean
    public CommandBus commandBus(Registry registry) {
        return new SpringCommandBus(registry);
    }
}


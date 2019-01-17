package pl.asc.tparegistercase.command.registercase;


import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import pl.asc.tparegistercase.cqs.Command;

@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class RegisterCaseCommand implements Command<RegisterCaseResult> {
    private String pesel;
    private String firstName;
    private String lastName;
    private String policyNr;
    private String city;
}

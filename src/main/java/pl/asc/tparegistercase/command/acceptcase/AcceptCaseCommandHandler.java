package pl.asc.tparegistercase.command.acceptcase;

import pl.asc.tparegistercase.cqs.CommandHandler;

public class AcceptCaseCommandHandler implements CommandHandler<AcceptCaseResult, AcceptCaseCommand> {
    @Override
    public AcceptCaseResult handle(AcceptCaseCommand command) {
        return new AcceptCaseResult(true); //FIXME
    }
}

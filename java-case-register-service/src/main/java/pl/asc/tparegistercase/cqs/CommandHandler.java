package pl.asc.tparegistercase.cqs;

public interface CommandHandler<R, C extends  Command<R>> {
    R handle(C command);
}

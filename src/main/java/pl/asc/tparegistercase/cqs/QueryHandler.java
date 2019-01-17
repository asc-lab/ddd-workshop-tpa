package pl.asc.tparegistercase.cqs;

public interface QueryHandler<R, C extends Query<R>> {
    R handle(C var1);
}

package pl.asc.tparegistercase.specification;


class SpecificationNotSatisfiedException extends RuntimeException {
    SpecificationNotSatisfiedException(String msg) {
        super(msg);
    }
}

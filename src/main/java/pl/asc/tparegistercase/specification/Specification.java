package pl.asc.tparegistercase.specification;

import lombok.Getter;

@Getter
public abstract class Specification<T> {
    private static final Object[] EMPTY_PARAMS = new Object[0];

    protected String errorMessage;

    public abstract boolean isSatisfiedBy(T objectToCheck);

    public void ensureIsSatisfiedBy(T objectToCheck) {
        if (!isSatisfiedBy(objectToCheck)) {
            //checkNotNull(getErrorCode(), "Error Code is required. Use failure(code, params) method");
            throw new SpecificationNotSatisfiedException(getErrorMessage());
        }
    }

}

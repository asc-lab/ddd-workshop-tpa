package pl.asc.tparegistercase.specification;

public class NotNullSpecification extends Specification<Object> {

    public NotNullSpecification(String errorMsg){
        this.errorMessage = errorMsg;
    }

    @Override
    public boolean isSatisfiedBy(Object objectToCheck) {
        return objectToCheck != null;
    }
}
